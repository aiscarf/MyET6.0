using System;

namespace Scarf.Moba
{
    public class BinaryHeap
    {
        public float growthFactor = 2f;
        private Cell[] binaryHeap;
        public int numberOfItems;

        public BinaryHeap(int numberOfElements)
        {
            this.binaryHeap = new Cell[numberOfElements];
            this.numberOfItems = 1;
        }

        public void Clear()
        {
            this.numberOfItems = 1;
        }

        public Cell GetNode(int nIndex)
        {
            return this.binaryHeap[nIndex];
        }

        public void Add(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException("Sending null node to BinaryHeap");
            if (this.numberOfItems == this.binaryHeap.Length)
            {
                Cell[] cellArray = new Cell[Math.Max(this.binaryHeap.Length + 4,
                    (int)Math.Round((double)this.binaryHeap.Length * (double)this.growthFactor))];
                for (int index = 0; index < this.binaryHeap.Length; ++index)
                    cellArray[index] = this.binaryHeap[index];
                this.binaryHeap = cellArray;
            }

            this.binaryHeap[this.numberOfItems] = cell;
            int index1 = this.numberOfItems;
            int f = cell.F;
            int index2;
            for (; index1 != 1; index1 = index2)
            {
                index2 = index1 >> 1;
                if (f < this.binaryHeap[index2].F)
                {
                    this.binaryHeap[index1] = this.binaryHeap[index2];
                    this.binaryHeap[index2] = cell;
                }
                else
                    break;
            }

            ++this.numberOfItems;
        }

        public Cell Remove()
        {
            if (this.numberOfItems < 2)
                return (Cell)null;
            --this.numberOfItems;
            Cell cell1 = this.binaryHeap[1];
            this.binaryHeap[1] = this.binaryHeap[this.numberOfItems];
            int index1 = 1;
            int index2;
            do
            {
                index2 = index1;
                int index3 = index2 << 1;
                int index4 = index3 + 1;
                if (index4 <= this.numberOfItems)
                {
                    if (this.binaryHeap[index1].F >= this.binaryHeap[index3].F)
                        index1 = index3;
                    if (this.binaryHeap[index1].F >= this.binaryHeap[index4].F)
                        index1 = index4;
                }

                if (index2 != index1)
                {
                    Cell cell2 = this.binaryHeap[index2];
                    this.binaryHeap[index2] = this.binaryHeap[index1];
                    this.binaryHeap[index1] = cell2;
                }
            } while (index2 != index1);

            return cell1;
        }

        private int GetPosIndex(Cell pos)
        {
            for (int index = 1; index < this.numberOfItems; ++index)
            {
                if (pos == this.binaryHeap[index])
                    return index;
            }

            return this.numberOfItems;
        }

        public void Rebuild(Cell pos)
        {
            for (int posIndex = this.GetPosIndex(pos); posIndex < this.numberOfItems; ++posIndex)
            {
                int index1 = posIndex;
                Cell cell = this.binaryHeap[posIndex];
                int f = cell.F;
                int index2;
                for (; index1 != 1; index1 = index2)
                {
                    index2 = index1 >> 1;
                    if (f < this.binaryHeap[index2].F)
                    {
                        this.binaryHeap[index1] = this.binaryHeap[index2];
                        this.binaryHeap[index2] = cell;
                    }
                    else
                        break;
                }
            }
        }
    }
}