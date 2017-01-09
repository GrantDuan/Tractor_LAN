using System;
using System.Resources;
using System.Drawing;
using Kuaff.CardResouces;

namespace Duan.Xiugang.Tractor
{
    [Serializable]
    class GameConfig
    {
        #region ʱ������

        //ʱ������
        int finishedOncePauseTime = DefinedConstant.FINISHEDONCEPAUSETIME; //����ʱ��

        internal int FinishedOncePauseTime
        {
            get { return finishedOncePauseTime; }
            set { finishedOncePauseTime = value; }
        }


        int noRankPauseTime = DefinedConstant.NORANKPAUSETIME; //������ͣʱ��

        public int NoRankPauseTime
        {
            get { return noRankPauseTime; }
            set { noRankPauseTime = value; }
        }

        int get8CardsTime = DefinedConstant.GET8CARDSTIME; //����ʱ��

        public int Get8CardsTime
        {
            get { return get8CardsTime; }
            set { get8CardsTime = value; }
        }
        int sortCardsTime = DefinedConstant.SORTCARDSTIME; //����ʱ��

        public int SortCardsTime
        {
            get { return sortCardsTime; }
            set { sortCardsTime = value; }
        }
        int finishedThisTime = DefinedConstant.FINISHEDTHISTIME; //�ܽ���ʱ��

        public int FinishedThisTime
        {
            get { return finishedThisTime; }
            set { finishedThisTime = value; }
        }

        int timerDiDa = DefinedConstant.TIMERDIDA; //��Ϸ�δ�

        public int TimerDiDa
        {
            get { return timerDiDa; }
            set { timerDiDa = value; }
        }
        #endregion // ʱ������


        #region ��������
        //�ش����
        private string mustRank = "";
        internal string MustRank
        {
            get { return mustRank; }
            set { mustRank = value; }
        }

        //�Ƿ��ڵ���
        private bool isDebug = false;
        internal bool IsDebug
        {
            get { return isDebug; }
            set { isDebug = value; }
        }


        //�۵��㷨
        private int bottomAlgorithm = 1;
        internal int BottomAlgorithm
        {
            get { return bottomAlgorithm; }
            set { bottomAlgorithm = value; }
        }

 
        //�Ƿ��������
        private bool isPass = true;
        internal bool IsPass
        {
            get { return isPass; }
            set { isPass = value; }
        }

        //�Ƿ����J����
        private bool jToBottom = false;
        internal bool JToBottom
        {
            get { return jToBottom; }
            set { jToBottom = value; }
        }

        //�Ƿ����q����
        private bool qToHalf = false;
        internal bool QToHalf
        {
            get { return qToHalf; }
            set { qToHalf = value; }
        }

        //�Ƿ����A��J
        private bool aToJ = false;
        internal bool AToJ
        {
            get { return aToJ; }
            set { aToJ = value; }
        }
        


        //�Ƿ�����Է�
        private bool canMyRankAgain = true;
        internal bool CanMyRankAgain
        {
            get { return canMyRankAgain; }
            set { canMyRankAgain = value; }
        }

        //�Ƿ����������
        private bool canRankJack = true;
        internal bool CanRankJack
        {
            get { return canRankJack; }
            set { canRankJack = value; }
        }

        //�Ƿ���Լӹ�
        private bool canMyStrengthen = true;
        internal bool CanMyStrengthen
        {
            get { return canMyStrengthen; }
            set { canMyStrengthen = value; }
        }

        private int whenFinished = 0;
        internal int WhenFinished
        {
            get { return whenFinished; }
            set { whenFinished = value; }
        }

        #endregion // ��������


        #region ͼ������
        //ֻ����ʹ�����õ�������Դʱ��������
        [NonSerialized()]
        ResourceManager cardsResourceManager = Kuaff_Cards.ResourceManager; //��ǰ������ʹ�õ���Դ������
        public ResourceManager CardsResourceManager
        {
            get {
                if (cardsResourceManager != null)
                {
                    return cardsResourceManager;
                }
                else
                {
                    cardsResourceManager = Kuaff_Cards.ResourceManager;
                    return cardsResourceManager;
                }
            }
            set { cardsResourceManager = value; }
        }

        [NonSerialized()]
        Bitmap backImage = Kuaff_Cards.back; //���汳��
        internal Bitmap BackImage
        {
            get {
                if (backImage != null)
                {
                    return backImage;
                }
                else
                {
                    backImage = Kuaff_Cards.back;
                    return backImage;
                }
            }
            set { backImage = value; }
        }
       
        //�Ƿ�����Զ��������
        private string cardImageName = ""; //����ͼ��
        internal string CardImageName
        {
            get { return cardImageName; }
            set { cardImageName = value; }
        }
        #endregion // ͼ������



    }
}
