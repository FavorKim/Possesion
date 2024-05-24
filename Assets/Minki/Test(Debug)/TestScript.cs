using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameObject[] monsters;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(monsters[Random.Range(0, monsters.Length)]);
        }
    }
}
