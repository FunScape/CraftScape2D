//
// Authored by Andrew Williams-Gilchrist, 2/20/2018
//

using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode
{
    //Used to determine what recipe is. id = -1 denotes a union node, id = -2 denotes an intersection node

    public Recipe recipe;
    public int getId() {
        if (recipe != null)
            return recipe.id;
        else if (isUnion)
            return -1;
        else
            return -2;
    }

    //If the recipe variable is null, this bool represents whether this is a union node or an intersection node.
    bool isUnion = false;

    //Lists what tier each skill is within the skill tree
    private int tier;
    public int getTier() { return this.tier; }
    public void setTier(int tier) { this.tier = tier; return; }

    //Indicates whether the player has unlocked this skill
    public bool getUnlocked() { return recipe.canCraft; }
    public void setUnlocked(bool canCraft) { recipe.canCraft = canCraft; }

    //A list of references to skillNodes that require this skill node to be unlocked before they can be unlocked
    public List<SkillNode> children;

    //A list of references to skillNodes that this skill require before it can be unlocked
    public List<SkillNode> dependencies;

    static string spritePath = "Sprites/";
    static string unionSpriteName = "union.png";
    static string intersectionSpriteName = "intersection.png";

    //Default constructor, for testing only
    public SkillNode()
    {
        recipe = new Recipe(0, null, 0, "", null, 0, 0, false);
        tier = -1;
        children = new List<SkillNode>();
        dependencies = new List<SkillNode>();
    }

    //For normal use
    //public SkillNode(int id, int tier, bool unlocked)
    public SkillNode(Recipe recipe)
    {
        this.recipe = recipe;
        tier = -1;
        children = new List<SkillNode>();
        dependencies = new List<SkillNode>();
    }

    //Constructs a dependency node. These nodes do not contain recipes. They represent a union or intersection dependency.
    public SkillNode(char dependencyType)
    {
        tier = -1;
        children = new List<SkillNode>();
        dependencies = new List<SkillNode>();

        if (dependencyType == 'U')
            isUnion = true;
        //isUnion defaults to false, so there's no need to set it here.
    }

    //Returns true if all dependencies have been unlocked, false otherwise
    public bool canBeUnlocked()
    {
        //If the node is a union node or a recipe node, all dependencies must be unlocked to unlock it.
        if (getId() >= -1)
        {
            foreach (SkillNode dep in dependencies)
            {
                if (!dep.getUnlocked())
                {
                    return false;
                }
            }

            return true;
        }
        //If the node id is less than -1 (i.e., -2), the node is an intersection, and a single dependecy being unlocked is sufficient.
        else
        {
            foreach (SkillNode dep in dependencies)
            {
                if (dep.getUnlocked())
                {
                    return true;
                }
            }

            return false;
        }
    }

    public SkillNode FindDependencyById(int dependencyId)
    {
        foreach(SkillNode dep in dependencies)
        {
            if (dep.getId() == dependencyId)
                return dep;
        }

        return null;
    }

    public Sprite getSprite()
    {
        if (getId() >= 0)
            return recipe.product.sprite;
        else if (getId() == -1)
            return (Sprite)Resources.Load(spritePath + unionSpriteName, typeof(Sprite));
        else if (getId() == -2)
            return (Sprite)Resources.Load(spritePath + intersectionSpriteName, typeof(Sprite));
        else
            return null;
    }
}