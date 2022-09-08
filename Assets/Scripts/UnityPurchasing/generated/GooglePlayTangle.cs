// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("5J/0BtaHZMjxLQxHD4H42/4p0p+7kp8FcljlReq8IYUA4/H0L8UuUNjIXs6Q9Aq+4Ruu8tjXqrzASCj2X7PaI24S2E2Q9LOcpcaqNwHLxhtVH7iMjOCEz3/e/qxCSko423MPZYYvdQtyWqguny3ORAxAAcmi7F4PVRdVF2+74iEeEp+xZr/gUQc+lmfyXkTz2Tu7wvPfn9odFCeo+mVGVNNh4sHT7uXqyWWrZRTu4uLi5uPgYeLs49Nh4unhYeLi4z5185p20qznNSCRgx0pU6VYG5AEAnhbgplYb411iB2Vj9gSD53SVGgHiJlyOiARM3/eJ0WCAYdJZLXtywNrH4CyezP9QV2fvPViNpNVH58duzT5oKGr63K7yDo4oOaG5OHg4uPi");
        private static int[] order = new int[] { 2,4,10,8,6,8,12,9,10,12,10,11,13,13,14 };
        private static int key = 227;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
