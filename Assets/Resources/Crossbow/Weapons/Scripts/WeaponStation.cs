using UnityEngine;
using System.Collections;

public class WeaponStation : MonoBehaviour, IUsable
{

    public Transform weaponPrefab;
    public Transform position;

    #region IUsable implementation
    public void Use(Object sender, RaycastHit raycastHit)
    {
        Transform player = sender as Transform;
        if (player != null)
        {
            // Get the player's weapon holder
            WeaponHolder weaponHolder = player.GetComponent<CrossbowController>().GetWeaponHolder();// player.GetComponentInChildren<PlayerExtScr>().GetWeaponHolder();
            // Instantiate a new weapon at the position of the weaponholder
            Transform prefabObject = Instantiate(weaponPrefab, weaponHolder.transform.position, Quaternion.identity) as Transform;
            // Try a carriable object
            ICarriable carriableItem = (ICarriable)prefabObject.GetComponent(typeof(ICarriable));
            // Is carriable?
            if (typeof(ICarriable).IsAssignableFrom(carriableItem.GetType()))
            {
                // Setup the carriable and attach an owner
                carriableItem.SetOwner(player);
                weaponHolder.ChangeItem(carriableItem);

                prefabObject.parent = player;
            }
        }
    }
    #endregion
}
