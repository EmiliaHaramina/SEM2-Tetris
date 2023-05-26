using System.Linq;
using UnityEngine;

public class QuestionController : MonoBehaviour
{

    public static int[] GenerateRandomOrder(int noOfElements)
    {
        System.Random random = new System.Random();
        int[] numbers = Enumerable.Range(0, noOfElements).ToArray();
        int[] randomOrder = numbers.OrderBy(x => random.Next()).ToArray();
        return randomOrder;
    }
}
