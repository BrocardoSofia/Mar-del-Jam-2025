using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int keys = 0;
    
    public void addKey()
    {
        keys++;
    }

    public void removeKey() 
    { 
        keys--; 
    }

    public int useKey()
    {
        int key;
        if (keys == 0)
            key = 0;
        else
        {
            key = 1;
            keys--;
        }

        return key;
    }
}
