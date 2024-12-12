namespace AdventOfCode2024.Models._11;

public class Stone(string numberString) : TreeNode
{
    public string NumberString { get; set; } = numberString;
    public long Number => long.Parse(NumberString);

    protected override void Update()
    {
        UpdateSelf();
        
        // Update children
        base.Update();
    }

    private void UpdateSelf()
    {
        if (NumberString.Contains('0'))
        {
            IsActive = false;
            var leftChild = new Stone();
            Add(leftChild);
            var rightChild = new Stone();
            Add(rightChild);
        }
    }
}