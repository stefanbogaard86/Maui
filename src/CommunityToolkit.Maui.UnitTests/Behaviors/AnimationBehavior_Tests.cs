﻿using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.UnitTests.Mocks;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Behaviors;

public class AnimationBehavior_Tests : BaseTest
{
	[Fact]
	public void TabGestureRecognizerAttachedWhenNoEventSpecified()
	{
		var boxView = new BoxView();
		boxView.Behaviors.Add(new AnimationBehavior());
		var gestureRecognizers = boxView.GestureRecognizers.ToList();

		Assert.Single(gestureRecognizers);
		Assert.IsType<TapGestureRecognizer>(gestureRecognizers[0]);
	}

	[Fact]
	public void TabGestureRecognizerNotAttachedWhenEventSpecified()
	{
		Assert.Throws<InvalidOperationException>(() => new BoxView().Behaviors.Add(new AnimationBehavior()
		{
			EventName = nameof(BoxView.Focused),
		}));
	}

	[Fact]
	public void TabGestureRecognizerNotAttachedWhenViewIsInputView()
	{
		Assert.Throws<InvalidOperationException>(() => new Entry().Behaviors.Add(new AnimationBehavior()));
	}

	[Fact]
	public async void AnimateCommandStartsAnimation()
	{
		bool animationStarted = false, animationEnded = false;

		var animationStartedTCS = new TaskCompletionSource();
		var animationEndedTCS = new TaskCompletionSource();

		var mockAnimation = new MockAnimation();
		mockAnimation.AnimationStarted += HandleAnimationStarted;
		mockAnimation.AnimationEnded += HandleAnimationEnded;

		var behavior = new AnimationBehavior
		{
			AnimationType = mockAnimation
		};

		new Label
		{
			Behaviors = { behavior }
		}.EnableAnimations();

		behavior.AnimateCommand.Execute(null);

		await animationStartedTCS.Task;
		await animationEndedTCS.Task;

		Assert.True(animationEnded);
		Assert.True(animationStarted);
		Assert.True(mockAnimation.HasAnimated);

		void HandleAnimationStarted(object? sender, EventArgs e)
		{
			mockAnimation.AnimationStarted -= HandleAnimationStarted;

			animationStarted = true;
			animationStartedTCS.SetResult();
		}

		void HandleAnimationEnded(object? sender, EventArgs e)
		{
			mockAnimation.AnimationEnded -= HandleAnimationEnded;

			animationEnded = true;
			animationEndedTCS.SetResult();
		}
	}

	class MockAnimation : BaseAnimation
	{
		public bool HasAnimated { get; private set; }

		public event EventHandler? AnimationStarted;
		public event EventHandler? AnimationEnded;

		public override async Task Animate(VisualElement? element)
		{
			ArgumentNullException.ThrowIfNull(element);

			AnimationStarted?.Invoke(this, EventArgs.Empty);

			await element.RotateTo(70);

			AnimationEnded?.Invoke(this, EventArgs.Empty);

			HasAnimated = true;
		}
	}
}
