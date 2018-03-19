using UnityEngine;

public interface movement{

    Vector2 nextMovement(movementManager.managerState state);

    string type();
}
