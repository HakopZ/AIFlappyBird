using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flappyBird
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        int counter = 0;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Bird bird;
        List<Pipes> pipes;
        TimeSpan pipeSpan;
        bool isUpdating = true;

        Random rand = new Random();
        float[][] inputs;
        float[][] outputs;
        int gap = 0;
        int gen = 0;

        KeyboardState ks;
        KeyboardState prevKs;
        Bird[] population;
        public Game1()
        { 
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            int RandY = rand.Next(-350, -100);
            pipes = new List<Pipes>();
            
            bird = new Bird(new Vector2(0, 0), Color.White, 0f, 0.085f, Content.Load<Texture2D>("flappy-bird"), new Vector2(50, 50), -0.6f);

            population = new Bird[1000];
            // TODO: use this.Content to load your game content here

            for (int x = 0; x < population.Length; x++)
            {
                population[x] = new Bird(new Vector2(0, 0), Color.White, 0f, 0.085f, Content.Load<Texture2D>("flappy-bird"), new Vector2(50, 50), -0.6f);


                population[x].Brain.Randomize(rand);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        Network TrainNet(float[][] inputs, float[][] desiredOutputs, int populationSize, int maxGeneration = -1)
        {
            (Network net, double mae)[] population = new(Network, double)[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                population[i] = (new Network(Activations.BinaryStep, 2, 2, 1), float.MaxValue);
                population[i].net.Randomize(rand);
            }
            int gen = 0;
            while (true)
            {
                for (int i = 0; i < population.Length; i++)
                {
                    population[i].mae = Mae(population[i].net, inputs, desiredOutputs);
                }
                Array.Sort(population, (a, b) => a.mae.CompareTo(b.mae));

                for (int i = 0; i < inputs.Length; i++)
                {
                    float[] output = population[0].net.Compute(inputs[i]);
                    if(output[0] == 1)
                    {
                        bird.Jump();
                    }
                }
                
                if (population[0].mae == 0 || gen == maxGeneration)
                {
                    break;
                }
                int end = (int)(population.Length * 0.90f);
                for (int i = 1; i < end; i++)
                {
                    population[i].net.Mutate(rand, 0.15);
                }
                for (int i = end; i < population.Length; i++)
                {
                    population[i].net.Randomize(rand);
                }
                gen++;
            }
            return population[0].net;
        }
         double Mae(Network net, float[][] input, float[][] DesiredOutput)
        {
            double mae = 0;
            for (int i = 0; i < input.Length; i++)
            {
                float[] output = net.Compute(input[i]);
                mae += output.Zip(DesiredOutput[i], (actual, expected) => Math.Abs(expected - actual)).Average();
            }
            return mae / input.Length;
        }
        void MakePipe(GameTime gameTime)
        {
            
                pipeSpan += gameTime.ElapsedGameTime;
                if (pipeSpan > TimeSpan.FromMilliseconds(1000) || pipes.Count == 0)
            
                {
                    pipeSpan = TimeSpan.Zero;
                    int RandomY = rand.Next(-350, -100);
                    pipes.Add(new Pipes(3f, Content.Load<Texture2D>("pipe"), new Vector2(GraphicsDevice.Viewport.Width, RandomY), Color.White, 0f));
                }
            
        }
        void NeverForget()
        {
            pipes.Clear();
            bird.HitPipe = false;
            bird.Position = new Vector2(50, 50);

            for (int i = 0; i < population.Length; i++)
            {
                population[i].HitPipe = false;
                population[i].Position = new Vector2(50, 50);
            }
            isUpdating = true;
            counter = 0;
        }
        protected override void Update(GameTime gameTime)
        {
            prevKs = ks;
            ks = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           
                MakePipe(gameTime);
                bird.Update(gameTime);
                
                for (int i = 0; i < pipes.Count; i++)
                {
                    pipes[i].Update(gameTime);
                    if (pipes[i].TopPipe.Hitbox.Intersects(bird.Hitbox) || pipes[i].BottomPipe.Hitbox.Intersects(bird.Hitbox))
                    {
                        bird.HitPipe = true;
                    }   
                    if (pipes[i].X + pipes[i].width < 0)
                    {
                        pipes.Remove(pipes[i]);
                        //break;
                    }
                    if (!pipes[i].isPassed && bird.Position.X > pipes[i].X)
                    {
                        pipes[i].isPassed = true;
                        counter++;
                    }
                }

                if (bird.Position.Y - bird.Hitbox.Height / 2 < 0 || bird.Position.Y + bird.Hitbox.Height / 2 > GraphicsDevice.Viewport.Height)
                {
                    bird.HitPipe = true;
                }
                var pipesAhead = pipes.Where(x => x.TopPipe.Hitbox.X + x.TopPipe.Hitbox.Width / 2 > bird.Hitbox.X).ToArray();
                Array.Sort(pipesAhead, (a, b) => a.Position.X.CompareTo(b.Position.X));
                var nextPipe = pipesAhead.FirstOrDefault();

                if (nextPipe != null)
                { 
                    for (int i = 0; i < population.Length; i++)
                    {

                    Vector2 distance = new Vector2(nextPipe.Position.X - population[i].Position.X, nextPipe.gap - population[i].Position.Y);
                    float[] output = population[i].Brain.Compute(new float[] { distance.X, distance.Y });

                        if (output[0] == 1)
                        {
                            population[i].Jump();
                        }
                    }
                }


                int casualties = 0;
                for (int i = 0; i < population.Length; i++)
                {
                    population[i].Update(gameTime);
                    if (population[i].HitPipe)
                    {
                        casualties++;
                    }
                    if (population[i].Hitbox.Y + population[i].Hitbox.Height / 2 < 0 || population[i].Hitbox.Y - population[i].Hitbox.Height / 2 > GraphicsDevice.Viewport.Height)
                    {
                        population[i].HitPipe = true;
                        continue;
                    }
                    for (int p = 0; p < pipes.Count; p++)
                    {
                        if (population[i].Hitbox.Intersects(pipes[p].TopPipe.Hitbox) || population[i].Hitbox.Intersects(pipes[p].BottomPipe.Hitbox))
                        {
                            population[i].HitPipe = true;
                            break;
                        }
                    }
                }
                //Network net = TrainNet(inputs, outputs, 1000);


                
                
                // TODO: Add your update logic here

                if(bird.HitPipe)
                {
                    bird.speed = new Vector2(0, 0);
                }
                bird.Update(gameTime);   
            
            if(casualties == population.Length)
            {
                NeverForget();
                
                gen++;
                int start = (int)(population.Length*0.10);
                int end = (int)(population.Length * 0.80);
                for (int i = start; i < end; i++)
                {
                    population[i].Brain.Mutate(rand, 0.15);
                }
                for (int i = end; i < population.Length; i++)
                {
                    population[i].Brain.Randomize(rand);
                }
                if (ks.IsKeyDown(Keys.Space) && prevKs.IsKeyUp(Keys.Space))
                {
                    bird.Jump();
                }
            }
            base.Update(gameTime);
        }
           

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            bird.Draw(spriteBatch);
            for (int i = 0; i < population.Length; i++)
            {
                population[i].Draw(spriteBatch);
            }
            foreach (Pipes currentPipes in pipes)
            {
                currentPipes.Draw(spriteBatch);
            }
            spriteBatch.DrawString(font, $"Score: {counter}", new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
