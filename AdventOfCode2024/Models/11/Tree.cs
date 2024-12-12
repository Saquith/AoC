namespace AdventOfCode2024.Models._11;

public class Tree<T> where T : TreeNode
{
    private List<T> _nodes = [];

    public long Count()
    {
        return _nodes.Select(n => n.Count()).Aggregate((a, b) => a + b);
    }

    public void Add(T node)
    {
        _nodes.Add(node);
    }
}

public abstract class TreeNode
{
    protected bool IsActive = true;
    private List<TreeNode> _children { get; } = [];

    public long Count()
    {
        return (IsActive ? 1 : 0) + _children.Select(c => c.Count()).Aggregate((a, b) => a + b);
    }
    
    public void Add(TreeNode child)
    {
        _children.Add(child);
    }

    protected virtual void Update()
    {
        Parallel.ForEach(_children, child => child.Update());
    }
}