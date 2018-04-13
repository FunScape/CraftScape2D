//
// Authored by Andrew Williams-Gilchrist, 2/20/2018
//

using System;
using System.Collections.Generic;

public class SkillNode
{
    //Used to determine what recipe is. id = -1 denotes a union node, id = -2 denotes an intersection node
    private int id;
    public int getId() { return this.id; }

    //Lists what tier each skill is within the skill tree
    private int tier;
    public int getTier() { return this.tier; }

    //Indicates whether the player has unlocked this skill
    private bool unlocked;
    public bool getUnlocked() { return this.unlocked; }
    public void setUnlocked(bool unlocked) { this.unlocked = unlocked; }

    //A list of references to skillNodes that require this skill node to be unlocked before they can be unlocked
    public List<SkillNode> children;

    //A list of references to skillNodes that this skill require before it can be unlocked
    public List<SkillNode> dependencies;

    //Default constructor, for testing only
    public SkillNode()
    {
        this.id = 0;
        this.children = new List<SkillNode> { };
        this.dependencies = new List<SkillNode> { };
    }

    //For normal use
    public SkillNode(int id, int tier, bool unlocked)
    {
        this.id = id;
        this.tier = tier;
        this.unlocked = unlocked;
        this.children = new List<SkillNode> { };
        this.dependencies = new List<SkillNode> { };
    }

    //Returns true if all dependencies have been unlocked, false otherwise
    public bool canBeUnlocked()
    {
        foreach(SkillNode dep in this.dependencies)
        {
            if(!dep.getUnlocked())
            {
                return false;
            }
        }

        return true;
    }
}