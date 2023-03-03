using Godot;
using System;

namespace Scripts.LevelGenerationFunctions
{
    public class RandomMethods
    {
        Random rand;

        public void createRandom(){
            rand = new Random(Guid.NewGuid().GetHashCode());
        }

        public int random(int minVal, int maxVal){
            return rand.Next(minVal, maxVal+1);
        }
    }
}
