// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("MbK8s4MxsrmxMbKys24lo8omgvyImA6ewKRa7rFL/qKIh/rskBh4pmMvjncV0lHXGTTlvZtTO0/Q4itjD+OKcz5CiB3ApOPM9Zb6Z1GblksFRwVHP+uycU5Cz+E277ABV27GN90l2E3F34hCX82CBDhX2MkianBB1n8lWyIK+H7PfZ4UXBBRmfK8Dl+3ZXDB0015A/UIS8BUUigL0skIP6IOFKOJa+uSo4/Pik1Ed/iqNRYE68LPVSIItRW67HHVULOhpH+VfgAFT+jc3LDUny+OrvwSGhpoiyNfNYMxspGDvrW6mTX7NUS+srKytrOwtM+kVobXNJihfVwXX9Goi655gs+tEQ3P7KUyZsMFT89N62Sp8PH7uyLrmGpo8LbWtLGwsrOy");
        private static int[] order = new int[] { 1,10,13,8,12,11,8,11,9,12,12,12,13,13,14 };
        private static int key = 179;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
