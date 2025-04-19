<Query Kind="Statements" />

int[] arr = { 46, 30, 75, 24, 51, 87, 65, 53, 65 };

SegmentTree segmentTree = new SegmentTree(arr);

"Originalno Segmentno Stablo:".Dump();
("Zbir intervala [0, 8]: " + segmentTree.Query(0, 8)).Dump();

"Lazy Update: Dodavanje 10 na interval [2, 5]".Dump();
segmentTree.RangeUpdate(2, 5, 10);
("Zbir intervala [0, 8] posle Lazy Update: " + segmentTree.Query(0, 8)).Dump();
("Zbir intervala [2, 5]: " + segmentTree.Query(2, 5)).Dump();

"Brisanje elementa na indeksu 3".Dump();
segmentTree.Delete(3);
("Zbir intervala [0, 8] nakon brisanja: " + segmentTree.Query(0, 8)).Dump();

"Ažuriranje elementa na indeksu 3 sa vrednošću 100...".Dump();
segmentTree.Update(3, 100);
("Zbir intervala [0, 8] nakon ažuriranja: " + segmentTree.Query(0, 8)).Dump();
("Zbir intervala [2, 5]: " + segmentTree.Query(2, 5)).Dump();


class SegmentTree
{
    private int[] tree;
    private int[] lazy;
    private int n;
    private int neutralValue;

    public SegmentTree(int[] arr, int neutralValue = 0)
    {
        n = arr.Length;
        this.neutralValue = neutralValue;
        tree = new int[4 * n];
        lazy = new int[4 * n];
        Build(0, 0, n - 1, arr);
    }

    private void Build(int node, int start, int end, int[] arr)
    {
        if (start == end)
        {
            tree[node] = arr[start];
        }
        else
        {
            int mid = (start + end) / 2;
            Build(2 * node + 1, start, mid, arr);
            Build(2 * node + 2, mid + 1, end, arr);
            tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
        }
    }

    private void Propagate(int node, int start, int end)
    {
        if (lazy[node] != 0)
        {
            tree[node] += (end - start + 1) * lazy[node];
            if (start != end)
            {
                lazy[2 * node + 1] += lazy[node];
                lazy[2 * node + 2] += lazy[node];
            }
            lazy[node] = 0;
        }
    }

    public int Query(int l, int r)
    {
        return Query(0, 0, n - 1, l, r);
    }

    private int Query(int node, int start, int end, int l, int r)
    {
        Propagate(node, start, end);

        if (r < start || l > end)
            return 0;

        if (l <= start && end <= r)
            return tree[node];

        int mid = (start + end) / 2;
        int left = Query(2 * node + 1, start, mid, l, r);
        int right = Query(2 * node + 2, mid + 1, end, l, r);
        return left + right;
    }

    public void RangeUpdate(int l, int r, int val)
    {
        RangeUpdate(0, 0, n - 1, l, r, val);
    }

    private void RangeUpdate(int node, int start, int end, int l, int r, int val)
    {
        Propagate(node, start, end);

        if (r < start || l > end)
            return;

        if (l <= start && end <= r)
        {
            lazy[node] += val;
            Propagate(node, start, end);
            return;
        }

        int mid = (start + end) / 2;
        RangeUpdate(2 * node + 1, start, mid, l, r, val);
        RangeUpdate(2 * node + 2, mid + 1, end, l, r, val);
        tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
    }

    public void Delete(int index)
    {
		if (index < 0 || index >= n)
    		{
        		throw new ArgumentOutOfRangeException(nameof(index), "Element je van opsega.");
    		}

    	Update(index, neutralValue);
    }

    public void Update(int index, int newValue)
    {
        Update(0, 0, n - 1, index, newValue);
    }

    private void Update(int node, int start, int end, int index, int newValue)
    {
        Propagate(node, start, end);

        if (start == end)
        {
            tree[node] = newValue;
            return;
        }

        int mid = (start + end) / 2;
        if (index <= mid)
            Update(2 * node + 1, start, mid, index, newValue);
        else
            Update(2 * node + 2, mid + 1, end, index, newValue);

        tree[node] = tree[2 * node + 1] + tree[2 * node + 2];
    }
}
