using UnityEngine;

namespace Base
{
    public static class TimeFreezer
    {
        public static void FreezeTime(float scale)
        {
            Time.timeScale = scale;
        }
    }
}
