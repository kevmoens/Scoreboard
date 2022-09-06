﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Scoreboard.UWP.Animation
{
    public class RadialGradients
    {

        // Colors used by the various radial gradient brushes.
        private static readonly Color s_innerRingCoolColor = Colors.BlueViolet;
        private static readonly Color s_outerRingCoolColor = Colors.LightBlue;
        private static readonly Color s_innerRingWarmColor = Colors.IndianRed;
        private static readonly Color s_outerRingWarmColor = Colors.OrangeRed;
        private static readonly Color s_pulseColor = Colors.AliceBlue;

        private bool IsRunning = false;
        private readonly Compositor _compositor = Window.Current.Compositor;
        private readonly SpriteVisual _pulsingBorderVisual;
        //private readonly CompositionColorGradientStop _stopwatchButtonInnerRingGradientStop;
        private readonly CompositionColorGradientStop _stopWatchButtonOuterRingGradientStop;
        private readonly CompositionColorGradientStop _pulsingBorderGradientStop1;
        private readonly CompositionColorGradientStop _pulsingBorderGradientStop2;

        public RadialGradients(UIElement userControl)
        {


            // Create Visuals to represent a round stopwatch button and a pulsing border.
            // The Visuals are all sized to 1x1 then scaled to the size of their container.

            // Create a circular clip so that the Visuals will be circles instead of squares.
            var circle = _compositor.CreateEllipseGeometry();
            circle.Radius = Vector2.One / 2;
            circle.Center = Vector2.One / 2;
            var circularClip = _compositor.CreateGeometricClip();
            circularClip.Geometry = circle;

            // Creates a 1x1 circular SpriteVisual.
            SpriteVisual CreateCircularSpriteVisual()
            {
                var result = _compositor.CreateSpriteVisual();
                result.Clip = circularClip;
                result.Size = Vector2.One;
                result.RelativeOffsetAdjustment = new Vector3(Vector2.One / 2, 0);
                result.AnchorPoint = Vector2.One / 2;
                return result;
            }

            var stopwatchButtonVisual = CreateCircularSpriteVisual();
            {
                var brush = _compositor.CreateRadialGradientBrush();
                brush.EllipseCenter = Vector2.One / 2;
                brush.EllipseRadius = Vector2.One / 2;

                //_stopwatchButtonInnerRingGradientStop = _compositor.CreateColorGradientStop();
                //_stopwatchButtonInnerRingGradientStop.Offset = 0.3f;
                //_stopwatchButtonInnerRingGradientStop.Color = s_innerRingCoolColor;
                //brush.ColorStops.Add(_stopwatchButtonInnerRingGradientStop);

                _stopWatchButtonOuterRingGradientStop = _compositor.CreateColorGradientStop();
                _stopWatchButtonOuterRingGradientStop.Offset = 0.75f;
                _stopWatchButtonOuterRingGradientStop.Color = s_outerRingCoolColor;
                //brush.ColorStops.Add(_stopWatchButtonOuterRingGradientStop);

                stopwatchButtonVisual.Brush = brush;
                stopwatchButtonVisual.Scale = Vector3.One * 0.8f;
            }

            _pulsingBorderVisual = CreateCircularSpriteVisual();
            {
                var brush = _compositor.CreateRadialGradientBrush();
                brush.EllipseCenter = Vector2.One / 2;
                brush.EllipseRadius = Vector2.One / 2;

                _pulsingBorderGradientStop1 = _compositor.CreateColorGradientStop();
                _pulsingBorderGradientStop1.Color = Colors.Transparent;
                brush.ColorStops.Add(_pulsingBorderGradientStop1);

                _pulsingBorderGradientStop2 = _compositor.CreateColorGradientStop();
                _pulsingBorderGradientStop2.Offset = 3; // KMM 1 - 3
                _pulsingBorderGradientStop2.Color = Colors.Transparent;
                brush.ColorStops.Add(_pulsingBorderGradientStop2);

                _pulsingBorderVisual.Brush = brush;
            }

            // Create a composition tree containing the 2 circular SpriteVisuals.
            var containerVisual = _compositor.CreateContainerVisual();
            containerVisual.Size = new Vector2(1, 2); // Vector2.One;
            containerVisual.Children.InsertAtTop(_pulsingBorderVisual);
            containerVisual.Children.InsertAtTop(stopwatchButtonVisual);

            // Parent the composition tree and automatically scale it to the size of the parent.
            var parentElement = userControl;
            ElementCompositionPreview.SetElementChildVisual(parentElement, containerVisual);
            var scaleAnimation = _compositor.CreateExpressionAnimation(
                "el.Size.X<el.Size.Y?Vector3(el.Size.X,el.Size.X,1):Vector3(el.Size.Y,el.Size.Y,1)");
            scaleAnimation.SetReferenceParameter("el", ElementCompositionPreview.GetElementVisual(parentElement));
            containerVisual.StartAnimation("Scale", scaleAnimation);
        }

        // Toggle the stopwatch between the running and stopped state.
        public async void Run()
        {
            if (!IsRunning)
            {
                // Animate the colors of the button to the warm colors to 
                // indicate that the stopwatch is running.
                var buttonColorAnimationDuration = TimeSpan.FromSeconds(2); //KMM 2 - 4
                {
                    var animation = _compositor.CreateColorKeyFrameAnimation();
                    animation.InsertKeyFrame(1, s_innerRingWarmColor);
                    animation.Duration = buttonColorAnimationDuration;
                    //_stopwatchButtonInnerRingGradientStop.StartAnimation("Color", animation);
                }

                {
                    var animation = _compositor.CreateColorKeyFrameAnimation();
                    animation.InsertKeyFrame(1, s_outerRingWarmColor);
                    animation.Duration = buttonColorAnimationDuration;
                    _stopWatchButtonOuterRingGradientStop.StartAnimation("Color", animation);
                }

                // Animate the pulsing border.
                // The offsets of both gradient stops are animated at the same rate, but the second 
                // one is started a quarter second after the first, and the first also animates its color.
                var pulseDuration = TimeSpan.FromSeconds(1); //KMM 1 - 2
                {
                    var animation = _compositor.CreateScalarKeyFrameAnimation();
                    animation.InsertKeyFrame(0, 0);
                    animation.InsertKeyFrame(1, 1);
                    animation.Duration = pulseDuration;
                    animation.IterationBehavior = AnimationIterationBehavior.Count;
                    animation.IterationCount = 1;
                    _pulsingBorderGradientStop1.StartAnimation("Offset", animation);

                    // Same animation, but delayed by a quarter second.
                    animation.DelayTime = TimeSpan.FromSeconds(0.25f);
                    _pulsingBorderGradientStop2.StartAnimation("Offset", animation);
                }

                // Make the pulse color visible.
                {
                    var animation = _compositor.CreateColorKeyFrameAnimation();
                    animation.InsertKeyFrame(0, Colors.Transparent);
                    animation.InsertKeyFrame(1, s_pulseColor);
                    animation.Duration = pulseDuration;
                    animation.IterationBehavior = AnimationIterationBehavior.Count;
                    animation.IterationCount = 1;
                    _pulsingBorderGradientStop1.StartAnimation("Color", animation);
                }

                // Animate the scale of the pulsing SpriteVisual so that it grows outward.
                {
                    var animation = _compositor.CreateVector3KeyFrameAnimation();
                    animation.InsertKeyFrame(0, Vector3.Zero);
                    animation.InsertKeyFrame(1, new Vector3(2, 2, 2)); //KMM Vector3.One -> new Vector3(2,2,2)
                    animation.Duration = pulseDuration;
                    animation.IterationBehavior = AnimationIterationBehavior.Count;
                    animation.IterationCount = 1;
                    _pulsingBorderVisual.StartAnimation("Scale", animation);
                }

                // Start updating the stopwatch display.                
                IsRunning = true;
                await Task.Delay(1000);
                Run(); //KMM STOP
            }
            else
            {
                // Animate button colors back to the cool colors.
                var cooldownDuration = TimeSpan.FromSeconds(2);
                {
                    var animation = _compositor.CreateColorKeyFrameAnimation();
                    animation.InsertKeyFrame(1, s_innerRingCoolColor);
                    animation.Duration = cooldownDuration;
                    //_stopwatchButtonInnerRingGradientStop.StartAnimation("Color", animation);
                }

                {
                    var animation = _compositor.CreateColorKeyFrameAnimation();
                    animation.InsertKeyFrame(1, s_outerRingCoolColor);
                    animation.Duration = cooldownDuration;
                    _stopWatchButtonOuterRingGradientStop.StartAnimation("Color", animation);
                }

                // Stop the pulsing.
                // Note that it is important to set all of the properties that were previously being
                // animated in order to actually stop the the system from animating them, even though
                // the animations are not visible.
                _pulsingBorderGradientStop1.Offset = 0;
                _pulsingBorderGradientStop2.Offset = 0;
                _pulsingBorderGradientStop1.Color = Colors.Transparent;
                _pulsingBorderVisual.Scale = Vector3.Zero;
                IsRunning = false;
            }
        }
    }
}
