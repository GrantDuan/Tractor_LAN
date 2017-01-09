using System;
using System.Collections.Generic;
using System.Text;

namespace TractorServer
{
    //������
    class DefinedConstant
    {
        //ʱ�䳣��
        internal const int FINISHEDONCEPAUSETIME = 1500; //ÿȦ��ͣʱ��
        internal const int NORANKPAUSETIME = 5000; //����ʱ��
        internal const int GET8CARDSTIME = 1000; //��8�ŵ��Ƶ�ʱ��
        internal const int SORTCARDSTIME = 1000; //�ҵ�������ʱ��
        internal const int FINISHEDTHISTIME = 2500; //ÿ����ͣʱ��
        internal const int TIMERDIDA = 100; //ϵͳ�δ�

    }


    /// <summary>
    /// ����״̬��ָʾ��һ������
    /// </summary>
    enum CardCommands
    {
        ReadyCards, //��������
        DrawCenter8Cards, //��8�ŵ��Ƶ�����
        WaitingForSending8Cards, //�ȴ��۵׵�����
        DrawMySortedCards,//�����ҵ��Ƶ�����

        Pause,//ͨ����ͣ����
        WaitingShowPass, //��ʾ���ֵ�����
        WaitingShowBottom, //�����Ƶ�����
        WaitingForSend, //�ȴ�����
        WaitingForMySending, //�ȴ��ҳ��Ƶ�����
        DrawOnceFinished,//����һȦ�������
        DrawOnceRank,//����һ�ֺ������
        Undefined //δ���������
    }

    /// <summary>
    /// ���浱ǰ��Ϸ״̬�Ķ���
    /// </summary>
    [Serializable]
    public struct CurrentState
    {
        /// <summary>
        /// �Լ���ǰ���ƾ�
        /// </summary>
        internal int OurCurrentRank;
        //������
        internal int OurTotalRound;

        /// <summary>
        /// �Է����ƾ�
        /// </summary>
        internal int OpposedCurrentRank;
        //������
        internal int OpposedTotalRound;

        /// <summary>
        /// ��ǰ����
        /// δ��0������1������2������3��÷��4������5
        /// </summary>
        internal int Trump;

        /// <summary>
        /// ��ǰ��ׯ��
        /// δ��0,�Լ�1���Լ�2����3����4
        /// </summary>
        internal int Master;



        internal CurrentState(int ourCurrentRank, int opposedCurrentRank, int trump, int master,int ourTotalRound,int opposedTotalRound)
        {
            OurCurrentRank = ourCurrentRank;
            OpposedCurrentRank = opposedCurrentRank;
            Trump = trump;
            Master = master;
            OurTotalRound = ourTotalRound;
            OpposedTotalRound = opposedTotalRound;
        }
    }
}
