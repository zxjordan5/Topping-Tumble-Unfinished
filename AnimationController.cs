// Don't Put me on the Spot, 4/3/2024
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToppingTumble
{
    internal class AnimationController
    {
        // Fields
        private int _currentFrame;
        private double _fps;
        private double _secondsPerFrame;
        private int _totalFrames;
        private int _rectangleX;
        private double _timer;
        private bool _playing;
        private bool _isFinished;

        // Properties
        /// <summary>
        /// Gets a source rectangle for the object to draw from
        /// 
        /// ASSUMES A 16X16 SIZE
        /// </summary>
        public Rectangle SourceRect
        {
            get
            {
                return new Rectangle(_rectangleX, _currentFrame * 16, 16, 16);
            }
        }

        // Constructors
        /// <summary>
        /// For use with a spritesheet for ONE OBJECT
        /// Creates a new animation controller
        /// </summary>
        /// <param name="totalFrames">How many frames of animation there are</param>
        /// <param name="fps">Frames per second</param>
        public AnimationController(int totalFrames, double fps)
        {
            // Set fields to parameters
            _totalFrames = totalFrames;
            _fps = fps;

            // Initialize other fields
            _rectangleX = 0;
            _timer = 0;
            _secondsPerFrame = 1 / fps;
            _playing = false;
            _isFinished = true;
        }

        /// <summary>
        /// For use with a spritesheet with MULTIPLE OBJECTS (ex: ingredient sheet)
        /// Creates a new animation controller
        /// </summary>
        /// <param name="totalFrames">How many frames of animation there are</param>
        /// <param name="fps">Frames per second</param>
        /// <param name="variant">Which object in the sheet to animate
        ///                       (0 = 1st object
        ///                        1 = 2nd object
        ///                        2 = 3rd object...)</param>
        public AnimationController(int totalFrames, double fps, int variant)
        {
            // Set fields to parameters
            _totalFrames = totalFrames;
            _fps = fps;
            _rectangleX = variant * 16;

            // Initialize other fields
            _timer = 0;
            _secondsPerFrame = 1 / fps;
            _playing = false;
            _isFinished = true;
        }

        // Methods

        /// <summary>
        /// Makes the animation play constantly
        /// </summary>
        public void Play()
        {
            _playing = true;
        }

        /// <summary>
        /// Makes the animation stop playing constantly
        /// </summary>
        public void Stop()
        {
            _playing = false;
        }

        /// <summary>
        /// Plays the animation one time only
        /// SHOULD NOT BE USED WHILE CONSTANTLY PLAYING
        /// </summary>
        public void PlayOnce()
        {
            _isFinished = false;
        }

        /// <summary>
        /// Updates the timer and current frame of animation
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Only add time if the animation is meant to be playing
            // or if it hasn't finished playing yet
            if (_playing || !_isFinished)
            {
                _timer += gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Check for moving to next frame
            if (_timer >= _secondsPerFrame)
            {
                // Move to the next frame
                _currentFrame++;

                // Remove a frame's worth of time from the timer
                _timer -= _secondsPerFrame;
            }

            // Make sure that the current frame is within bounds
            if (_currentFrame >= _totalFrames)
            {
                // Reset the frames
                _currentFrame = 0;

                // Set the animation to be finished
                _isFinished = true;
            }
        }
    }
}
