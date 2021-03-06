using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public class BaseBlueprint : Resource
{
    [Export]
    public string name { get; set; } = "BaseBlueprint";

    public Texture texture { get; set; }
    //A blueprint is made up of a list of required pieces
    [Export(PropertyHint.Enum)]
    public Array<Parts.PartType> requiredPieces = new Array<Parts.PartType>{};

    //Returns bool on if its craftable and if not returns a list of piece types that are needed
    public Tuple<bool,Array<Parts.PartType>> IsCraftableWithGivenMaterials(Array<Parts.PartBlueprint> existingPieces)
    {
        //create copies
        List<long> usedPieces = new List<long>();

        //List of missing pieces
        Array<Parts.PartType> missingPieces = new Array<Parts.PartType>();
        
        //For every piece
        foreach (var requiredPiece in requiredPieces)
        {
            bool foundPiece = false;
            //check through the existing materials
            foreach (var existingPiece in existingPieces)
            {
                //If we have the correct piece type and its not contained within the already used materials
                if(existingPiece.partType == requiredPiece && !usedPieces.Contains(existingPiece.uuid))
                {
                    //add it to the list of used materials and break searching for the current piece
                    usedPieces.Add(existingPiece.uuid);
                    foundPiece = true;
                    break;
                }
            }

            //If we get to the end of the materials and we have not found a material then add a missing material to the list
            if(foundPiece == false)
            {
                missingPieces.Add(requiredPiece);
            }
        }
        
        //Return both the bool on if craftable and the list of pieces that are needed
        return new Tuple<bool,Array<Parts.PartType>>(missingPieces.Count == 0, missingPieces);
    }
}
