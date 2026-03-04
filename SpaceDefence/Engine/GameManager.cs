using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceDefence
{
    public class GameManager
    {
        private bool isDead = false;
        private static GameManager gameManager;

        private List<GameObject> _gameObjects;
        private List<GameObject> _toBeRemoved;
        private List<GameObject> _toBeAdded;
        private ContentManager _content;
        private SpriteFont font;

        public Random RNG { get; private set; }
        public Camera Camera { get; private set; }
        public Ship Player { get; private set; }
        public InputManager InputManager { get; private set; }
        public Game Game { get; private set; }

        public static GameManager GetGameManager()
        {
            if (gameManager == null)
                gameManager = new GameManager();
            return gameManager;
        }

        public GameManager()
        {
            _gameObjects = new List<GameObject>();
            _toBeRemoved = new List<GameObject>();
            _toBeAdded = new List<GameObject>();
            InputManager = new InputManager();
            RNG = new Random();
        }

        public void Initialize(ContentManager content, Game game, Ship player, Camera camera)
        {
            Game = game;
            _content = content;
            Player = player;
            Camera = camera;
        }

        public void Load(ContentManager content)
        {
            font = content.Load<SpriteFont>("PixelFont");
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Load(content);
            }
        }

        public void HandleInput(InputManager inputManager)
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.HandleInput(this.InputManager);
            }
            // reset if + or space is pressed
            if (
                isDead
                && (
                    inputManager.IsKeyDown(Keys.Space)
                    || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
                )
            )
            {
                isDead = false;
                Player.ResetPosition();
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                Player.ResetPosition();
        }

        public void CheckCollision()
        {
            // Checks once for every pair of 2 GameObjects if the collide.
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                for (int j = i + 1; j < _gameObjects.Count; j++)
                {
                    if (_gameObjects[i].CheckCollision(_gameObjects[j]))
                    {
                        _gameObjects[i].OnCollision(_gameObjects[j]);
                        _gameObjects[j].OnCollision(_gameObjects[i]);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update();

            // Handle input
            HandleInput(InputManager);

            if (isDead) // the game stops running during the death screen, save for handling input
                return;

            // Update
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Update(gameTime);
            }

            // Check Collission
            CheckCollision();

            foreach (GameObject gameObject in _toBeAdded)
            {
                gameObject.Load(_content);
                _gameObjects.Add(gameObject);
            }
            _toBeAdded.Clear();

            foreach (GameObject gameObject in _toBeRemoved)
            {
                gameObject.Destroy();
                _gameObjects.Remove(gameObject);
            }
            _toBeRemoved.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Camera.CenterCameraToWorldPosition(Player.GetPosition().Location.ToVector2());
            spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: Camera.GetScreenSpaceMatrix()
            );
            var screen = Game.GraphicsDevice.Viewport.Bounds;
            if (isDead)
            {
                string gameOverText = "GAME OVER";
                Vector2 goTextSize = font.MeasureString(gameOverText);
                Vector2 posA = screen.Center.ToVector2() - goTextSize * 2;
                spriteBatch.DrawString(
                    font,
                    gameOverText,
                    posA,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Vector2.One * 4,
                    SpriteEffects.None,
                    0
                );
                string outputB = "Press  spacebar  or (A)  to  continue.";
                Vector2 sizeB = font.MeasureString(outputB);
                Vector2 posB = screen.Center.ToVector2() - sizeB * 2 + (sizeB * 3 * Vector2.UnitY); //screen.Center.ToVector2() - 0.5f * Vector2.UnitX * font.MeasureString(outputB) + Vector2.UnitY * sizeA * 2;
                spriteBatch.DrawString(
                    font,
                    outputB,
                    posB,
                    Color.White,
                    0,
                    Vector2.Zero,
                    Vector2.One * 4,
                    SpriteEffects.None,
                    0
                );
                spriteBatch.End();
                return;
            }
            else
            {
                foreach (GameObject gameObject in _gameObjects)
                {
                    gameObject.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Add a new GameObject to the GameManager.
        /// The GameObject will be added at the start of the next Update step.
        /// Once it is added, the GameManager will ensure all steps of the game loop will be called on the object automatically.
        /// </summary>
        /// <param name="gameObject"> The GameObject to add. </param>
        public void AddGameObject(GameObject gameObject)
        {
            _toBeAdded.Add(gameObject);
        }

        /// <summary>
        /// Remove GameObject from the GameManager.
        /// The GameObject will be removed at the start of the next Update step and its Destroy() mehtod will be called.
        /// After that the object will no longer receive any updates.
        /// </summary>
        /// <param name="gameObject"> The GameObject to Remove. </param>
        public void RemoveGameObject(GameObject gameObject)
        {
            _toBeRemoved.Add(gameObject);
        }

        /// <summary>
        /// Get a random location on the screen.
        /// </summary>
        public Vector2 RandomScreenLocation()
        {
            return new Vector2(
                RNG.Next(0, Game.GraphicsDevice.Viewport.Width),
                RNG.Next(0, Game.GraphicsDevice.Viewport.Height)
            );
        }

        public void Death()
        {
            isDead = true;
        }

        public Rectangle GetScreenDimensions() => Game.GraphicsDevice.Viewport.Bounds;
    }
}
