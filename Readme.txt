Barliesque.Ease - Quick Overview

--------------------------------

The following line will call AnimateSomething() every frame for the next three seconds, passing one float parameter that changes from 0.0 to 1.0:

this.Play(3f, AnimateSomething);

private void AnimateSomething(float t)
{
	// You can apply an easing formula, and transition anything you want.
	// This version of the Ease function accepts and returns 
	// a value from 0.0 to 1.0
	var eased = Ease.Cubic.Out(t);
	AdjustSomething(eased);
}

--------------------------------

This will call a different version of AnimateSomething() that accepts a couple of parameters.

this.Play(3f, AnimateSomething, new Vector2(0, 0), new Vector2(100f, 100f));

private void AnimateSomething(float t, Vector2 oldPos, Vector2 newPos)
{
	var eased = Vector2.Lerp(oldPos, newPos, Ease.Cubic.InOut(t));
	AdjustPosition(eased);
}

--------------------------------

Alternatively...

private void AnimateSomething(float t, Vector2 oldPos, Vector2 newPos)
{
	// This version of the Ease function transforms a value from 0-1 
	// to whatever range you want.
	var easedX = Ease.Cubic.In(t, oldPos.x, newPos.x);
	var easedY = Ease.Cubic.Out(t, oldPos.y, newPos.y);
	AdjustPosition(new Vector2(easedX, easedY));
}

--------------------------------

You can also allow selection of the Ease Type from a parameter in the Inspector...

[SerializeField] private EaseSpec _howToEase;


To use the EaseSpec variable...

var eased = Ease.Call(_howToEase, 0.1f);

--------------------------------

this.Play() returns a CoroutineHandle.  Hang on to that if you want to be able to stop that coroutine from playing.  This is not Unity's Coroutine system, but a 3rd party library called MEC (More Effective Coroutines Pro)  More info about that here:  http://trinary.tech/category/mec/

Q:  Do I need to extend a specific class that has the Play call?
A:  No.  These methods are found in the namespaces: Barliesque.Utils and Barliesque.Ease

Q:  Can it be used inline?  
A:  Yes!  Like, so:  this.Play(0.5f, (float value) => {});

Q:  How do I stop the animation mid-way?
A:  Using the CoroutineHandle returned from this.Play() you can do this:
Timing.KillCoroutines(handle);

You can also check handle.IsRunning

You can even pause/unpause with: handle.IsAliveAndPaused = false or true 

