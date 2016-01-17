using System.Threading;
using UnityEngine;

namespace MOTD
{
    public class Tps
    { 
        private float tps1Min, tps5Min, tps15Min;

        public float get1Min() { return tps1Min; }
        public float get5Min() { return tps5Min; }
        public float get15Min() { return tps15Min; }

        public  Tps()
        {
            Thread firstTPSUpdate = new Thread(UpdateTPSOnStart);
            Thread tpsUpdateThread = new Thread(TPSUpdate);

            firstTPSUpdate.Start();
            tpsUpdateThread.Start();
        }

        private void UpdateTPSOnStart() // updates tps until 1 min left after server start
        {
            for (int i = 0; i < 55; i++)
            {
                float currentTPS = Time.timeScale / Time.deltaTime;
                tps1Min = (currentTPS + tps1Min) / 2;
                tps5Min = tps1Min;
                tps15Min = tps1Min;
                Thread.Sleep(1000);
            }
        }

        private void TPSUpdate()
        {
            float tpsSum = 0;
            int tpsCount = 0;
            float[] masivTPS = new float[15];

            while (true)
            {
                tpsSum += Time.timeScale / Time.deltaTime;
                tpsCount++;

                if (tpsCount == 60)
                {
                    for (int i = 14; i > 0; i--)
                    {
                        masivTPS[i] = masivTPS[i - 1];
                    }

                    masivTPS[0] = tpsSum / tpsCount;
                    ConvertToMins(masivTPS);

                    tpsSum = 0;
                    tpsCount = 0;
                }
                
                Thread.Sleep(1000); // updates ones per second
            }
        }

        private void ConvertToMins(float[] masivTPS)
        {
            tps1Min = Calc(masivTPS, 1);
            tps5Min = Calc(masivTPS, 5);
            tps15Min = Calc(masivTPS, 15);
        }

        private float Calc(float[] masivTPS, int amount)
        {
            int k = amount;
            float val = 0;

            for (int i = 0; i < amount; i++)
            {
                if (masivTPS[i] == 0)
                {
                    k--;
                }
                val += masivTPS[i];
            }

            return val / k;
        }
    }
}
