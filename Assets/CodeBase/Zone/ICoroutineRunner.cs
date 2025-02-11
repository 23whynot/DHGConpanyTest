using System.Collections;
using UnityEngine;

namespace CodeBase.Zone
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}