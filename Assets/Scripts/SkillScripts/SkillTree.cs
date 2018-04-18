//
// Authored by Andrew Williams-Gilchrist, 2/20/2018
//

using System;
using System.Collections.Generic;

public class SkillTree
{
    //Contains a full list of skillNodes managed by this SkillTree
    public List<SkillNode> skills;

    //Number of internal tiers
    private int tierSize;
    public int getTierSize() { return this.tierSize; }

    public SkillTree(int tierSize)
    {
        this.tierSize = tierSize;
        this.skills = generateContent();

        topologicalSort();
    }

    //Will eventually read from a database to create a bunch of SkillNodes
    //Right now, creates sample list for testing
    public static List<SkillNode> generateContent()
    {
        List<SkillNode> content = new List<SkillNode>();
        
        SkillNode pickaxe = new SkillNode(1, 1, false);
        SkillNode candle = new SkillNode(2, 2, false);
        SkillNode dagger = new SkillNode(3, 2, false);
        SkillNode ring = new SkillNode(4, 2, false);

        pickaxe.children.Add(candle);
        pickaxe.children.Add(dagger);
        pickaxe.children.Add(ring);

        foreach(SkillNode ch in pickaxe.children)
        {
            ch.dependencies.Add(pickaxe);
        }

        content.Add(pickaxe);
        content.Add(candle);
        content.Add(dagger);
        content.Add(ring);

        return content;
    }

    public void topologicalSort()
    {
        List<SkillNode> sortedSkills = new List<SkillNode>();

        Dictionary<int, bool> visitedDict = new Dictionary<int, bool>();

        foreach (SkillNode node in skills)
        {
            visitedDict.Add(node.getId(), false);
        }

        foreach (SkillNode node in skills)
        {
            if (!visitedDict[node.getId()])
                topologicalSortRecurse(ref sortedSkills, ref visitedDict, node);
        }

        skills = sortedSkills;
    }

    public void topologicalSortRecurse(ref List<SkillNode> sortedSkills, ref Dictionary<int, bool> visitedDict, SkillNode node)
    {
        visitedDict[node.getId()] = true;

        foreach (SkillNode child in node.children)
        {
            if (!visitedDict[child.getId()])
                topologicalSortRecurse(ref sortedSkills, ref visitedDict, child);
        }

        sortedSkills.Add(node);
    }
}