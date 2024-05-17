using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTestCube : MonoBehaviour
{
    [SerializeField] private GameObject _testCube;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_testCube);
        }
    }
}
