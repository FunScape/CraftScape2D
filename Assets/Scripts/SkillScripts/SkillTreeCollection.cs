//
// Authored by Andrew Williams-Gilchrist, 2/20/2018
//

using System;
using System.Collections.Generic;

public class SkillTreeCollection
{
    //Contains the associated skill trees for this skill
    public List<SkillTree> subtrees;

    public SkillTreeCollection()
    {
        this.subtrees = new List<SkillTree> { };
    }
}