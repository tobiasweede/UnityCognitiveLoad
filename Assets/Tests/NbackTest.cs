using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NbackTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void NbackTestSimplePasses()
    {
        // Use the Assert class to test conditions
        var go = GameObject.Find("Nback");
        Assert.IsNotNull(go);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NbackTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
