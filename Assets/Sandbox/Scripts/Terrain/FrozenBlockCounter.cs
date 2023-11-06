using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenBlockCounter : MonoBehaviour
{
    [SerializeField] GameObject _instantiatedBlock;

    private void Start()
    {
        _instantiatedBlock = null;
    }

    public void SetNewBlock(GameObject block)
    {
        if(_instantiatedBlock is null)
        {
            _instantiatedBlock = block;
        }
        else
        {
            ReplaceBlock(block);
        }
    }

    private void ReplaceBlock(GameObject block)
    {
        Destroy(_instantiatedBlock);
        _instantiatedBlock = block;
    }
}
