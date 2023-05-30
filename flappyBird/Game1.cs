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

        Random rand = new Random();
        double[][] inputs;
        double[][] outputs;
        int gen = 0;
        double mutateRate = 0.01;
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
            pipes = new List<Pipes>();

            bird = new Bird(new Vector2(0, 0), Color.White, 0f, 0.085f, Content.Load<Texture2D>("flappy-bird"), new Vector2(50, 50), -0.6f);

            population = new Bird[1000];
            // TODO: use this.Content to load your game content here

            for (int x = 0; x < population.Length; x++)
            {
                population[x] = new Bird(new Vector2(0, 0), Color.White, 0f, 0.085f, Content.Load<Texture2D>("flappy-bird"), new Vector2(50, GraphicsDevice.Viewport.Y / 2), -0.6f);


                population[x].Brain.Randomize(rand, -2, 2);
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
                population[i].Fitness = 0;
                
                population[i].Position = new Vector2(50, GraphicsDevice.Viewport.Y / 2);
            }
            counter = 0;
        }
        protected override void Update(GameTime gameTime)
        {
            prevKs = ks;
            ks = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            MakePipe(gameTime);
            
            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].Update(gameTime);
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

                    Vector2 distance = new Vector2(nextPipe.Position.X - population[i].Position.X, (float)nextPipe.gap - population[i].Position.Y);
                    double[] output = population[i].Brain.Compute(new double[] { distance.X, distance.Y });

                    if (output[0] == 1)
                    {
                        population[i].Jump();
                    }
                }
            }
            
            for (int i = 0; i < population.Length; i++)
            {
                population[i].Update(gameTime);
               
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
                if(!population[i].HitPipe)
                    population[i].Fitness++;
            }
            foreach(var pipe in pipes)
            {
                if (!pipe.isPassed && population[0].Position.X > pipe.X)
                {
                    pipe.isPassed = true;
                    counter++;
                }
            }
            
            if (bird.HitPipe)
            {
                bird.speed = new Vector2(0, 0);
            }
            bird.Update(gameTime);
            bool done = true;
            foreach(var b in population)
            {
                if (!b.HitPipe)
                {
                    done = false;
                    break;
                }
            }
            if (done)
            {
                GeneticLearning.Train(population, rand, mutateRate);
                NeverForget();

                gen++;



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
            //bird.Draw(spriteBatch);
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
