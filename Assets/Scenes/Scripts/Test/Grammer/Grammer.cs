using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grammer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Queue<int> asd = new Queue<int>();

    }

    // Update is called once per frame
    void Update()
    { 

    }

    public int solution(int[] priorities, int location)
    {
        int result = 0;
        Queue<int> sequence = new Queue<int>();
        Queue<int> wait = new Queue<int>();
        Queue<int> temp = new Queue<int>();

        //대기열 저장
        for (int i = 0; i < priorities.Length; i++)
        {
            wait.Enqueue(i);
        }
        while (sequence.Count < priorities.Length)
        {
            int index = wait.Dequeue();
            int big = priorities[index];
            temp.Enqueue(index);
            //대기 목록에 크기를 비교 후 마지막 값 찾기
            if (wait.Count != 1)
            {
                for (int i = 0; i < wait.Count; i++)
                {
                    int next_Index = wait.Dequeue();
                    temp.Enqueue(next_Index);
                    if (big < priorities[next_Index])
                    {
                        big = priorities[next_Index];
                        index = next_Index;
                    }
                }
            }
            //마지막 대기 목록은 그냥 저장
            else
            {
                sequence.Enqueue(index);
            }
            // temp 목록에 index 빼오고 wait 목록에 저장 후 다시 실행
            for (int i = 0; i < temp.Count; i++)
            {
                int num = temp.Dequeue();
                if (num == index)
                {
                    sequence.Enqueue(index);
                }
                else
                {
                    wait.Enqueue(num);
                }
            }
        }

        //목록 반환
        for (int i = 0; i < sequence.Count; i++)
        {
            int text = sequence.Dequeue();
            if (location == text)
            {
                result = i;
            }
        }
        return result;
    }
}
