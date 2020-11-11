using Assimp;
using System;
using System.Linq;
/// THIS IS BASED ON WORK BY:  WIL MOTIL  (a slightly older modified version)
/// https://github.com/willmotil/MonoGameUtilityClasses

// TO DO: 
// For Minimal Info (bottom), we may want to later add more texture type info as we add support for more texture types. 
// (The code in here is a bit crazy to look at, if desired, one could always add some white space - Wil's version is a bit cleaner)

namespace Game3D.SkinModels.SkinModelHelpers
{
    class LoadDebugInfo
    {
        SkinModelLoader ld;

        // CONSTRUCT
        public LoadDebugInfo(SkinModelLoader inst) {
            ld = inst;
        }

        // WRITE HEADER
        public void WriteHeader(string msg){
            Console.Write("\n______________"); Console.Write($" {msg}"); Console.Write("\n______________ \n");
        }

        
        #region CONSOLE ASSIMP OUTPUT (mostly): 

        // ASSIMP SCENE DUMP 
        public void AssimpSceneDump(Scene scene)
        {
            if (!ld.AssimpInfo) return;
            Console.Write("\n\n_______________________________________________");
            Console.Write("\n ---------------------");
            Console.Write("\n AssimpSceneDump...");
            Console.Write("\n --------------------- \n ");
            Console.Write("\n Model Name: " + ld.FilePathName);
            Console.Write("\n scene.CameraCount: " + scene.CameraCount);
            Console.Write("\n scene.LightCount: " + scene.LightCount);
            Console.Write("\n scene.MeshCount: " + scene.MeshCount);
            Console.Write("\n scene.MaterialCount: " + scene.MaterialCount);
            Console.Write("\n scene.TextureCount: " + scene.TextureCount + "(embedded data)");
            Console.Write("\n scene.AnimationCount: " + scene.AnimationCount);
            Console.Write("\n scene.RootNode.Name: " + scene.RootNode.Name); Console.Write("\n \n ");
            Console.Write("\n ///////////////////////////////////////////////////");
            Console.Write("\n scene.Lights");
            var aiLights = scene.Lights;
            for (int i = 0; i < aiLights.Count; i++) { var aiLight = aiLights[i];
                Console.Write("\n aiLight " + i + " of " + (aiLights.Count - 1) + "");
                Console.Write("\n aiLight.Name: " + aiLight.Name);
            }                
            Console.Write("\n \n ///////////////////////////////////////////////////");
            Console.Write("\n scene.Cameras");
            var aiCameras = scene.Cameras;
            for (int i = 0; i < aiCameras.Count; i++) { var aiCamera = aiCameras[i];
                Console.Write("\n aiCamera " + i + " of " + (aiCameras.Count - 1) + "");
                Console.Write("\n aiCamera.Name: " + aiCamera.Name);
            }                
            Console.Write("\n \n ///////////////////////////////////////////////////");
            Console.Write("\n scene.Meshes");
            var aiMeshes = scene.Meshes;
            for (int i = 0; i < aiMeshes.Count; i++) {
                var aiMesh = aiMeshes[i];                    
                Console.Write("\n \n --------------------------------------------------");
                Console.Write("\n Mesh " + i + " of " + (aiMeshes.Count - 1) + "");
                Console.Write("\n aiMesh.Name: " + aiMesh.Name);
                Console.Write("\n aiMesh.VertexCount: " + aiMesh.VertexCount);
                Console.Write("\n aiMesh.FaceCount: " + aiMesh.FaceCount);
                Console.Write("\n aiMesh.Normals.Count: " + aiMesh.Normals.Count);
                Console.Write("\n aiMesh.MorphMethod: " + aiMesh.MorphMethod);
                Console.Write("\n aiMesh.MaterialIndex: " + aiMesh.MaterialIndex);
                Console.Write("\n aiMesh.MeshAnimationAttachmentCount: " + aiMesh.MeshAnimationAttachmentCount);
                Console.Write("\n aiMesh.Tangents.Count: " + aiMesh.Tangents.Count);
                Console.Write("\n aiMesh.BiTangents.Count: " + aiMesh.BiTangents.Count);
                Console.Write("\n aiMesh.VertexColorChannelCount: " + aiMesh.VertexColorChannelCount);
                Console.Write("\n aiMesh.UVComponentCount.Length: " + aiMesh.UVComponentCount.Length);
                Console.Write("\n aiMesh.TextureCoordinateChannelCount: " + aiMesh.TextureCoordinateChannelCount);
                for (int k = 0; k < aiMesh.TextureCoordinateChannels.Length; k++) {
                    if (aiMesh.TextureCoordinateChannels[k].Count() > 0) Console.Write("\n aiMesh.TextureCoordinateChannels[" + k + "].Count(): " + aiMesh.TextureCoordinateChannels[k].Count());
                }
                Console.Write("\n aiMesh.BoneCount: " + aiMesh.BoneCount);                    
                Console.Write("\n \n Bones store a vertex id and a vertex weight. \n ");                    
                for (int b = 0; b < aiMesh.Bones.Count; b++) {
                    var aiMeshBone = aiMesh.Bones[b];
                    Console.Write("\n  aiMesh Bone " + b + " of " + (aiMesh.Bones.Count - 1) + "  aiMeshBone.Name: " + aiMeshBone.Name + "      aiMeshBone.VertexWeightCount: " + aiMeshBone.VertexWeightCount);
                    if (aiMeshBone.VertexWeightCount > 0) Console.Write("    aiMeshBone.VertexWeights[0]VertexID: " + aiMeshBone.VertexWeights[0].VertexID);                        
                } Console.Write("");
            }                
            Console.Write("\n \n ///////////////////////////////////////////////////");
            Console.Write("\n scene.Materials");
            var aiMaterials = scene.Materials;
            for (int i = 0; i < aiMaterials.Count; i++) {
                var aiMaterial = aiMaterials[i];                    
                Console.Write("\n \n --------------------------------------------------");
                Console.Write("\n " + "aiMaterial " + i + " of " + (aiMaterials.Count - 1) + "");
                Console.Write("\n " + "aiMaterial.Name: " + aiMaterial.Name);
                Console.Write("\n " + "ColorAmbient: " + aiMaterial.ColorAmbient + "  ColorDiffuse: " + aiMaterial.ColorDiffuse + "  ColorSpecular: " + aiMaterial.ColorSpecular);
                Console.Write("\n " + "ColorEmissive: " + aiMaterial.ColorEmissive + "  ColorReflective: " + aiMaterial.ColorReflective + "  ColorTransparent: " + aiMaterial.ColorTransparent);
                Console.Write("\n " + "Opacity: " + aiMaterial.Opacity + "  Shininess: " + aiMaterial.Shininess + "  ShininessStrength: " + aiMaterial.ShininessStrength);
                Console.Write("\n " + "Reflectivity: " + aiMaterial.Reflectivity + "  ShadingMode: " + aiMaterial.ShadingMode + "  BlendMode: " + aiMaterial.BlendMode + "  BumpScaling: " + aiMaterial.BumpScaling);
                Console.Write("\n " + "IsTwoSided: " + aiMaterial.IsTwoSided + "  IsWireFrameEnabled: " + aiMaterial.IsWireFrameEnabled);
                Console.Write("\n " + "HasTextureAmbient: " + aiMaterial.HasTextureAmbient + "  HasTextureDiffuse: " + aiMaterial.HasTextureDiffuse + "  HasTextureSpecular: " + aiMaterial.HasTextureSpecular);
                Console.Write("\n " + "HasTextureNormal: " + aiMaterial.HasTextureNormal + "  HasTextureDisplacement: " + aiMaterial.HasTextureDisplacement + "  HasTextureHeight: " + aiMaterial.HasTextureHeight + "  HasTextureLightMap: " + aiMaterial.HasTextureLightMap);
                Console.Write("\n " + "HasTextureEmissive: " + aiMaterial.HasTextureEmissive + "  HasTextureOpacity: " + aiMaterial.HasTextureOpacity + "  HasTextureReflection: " + aiMaterial.HasTextureReflection); Console.Write("\n");
                // https://github.com/assimp/assimp/issues/3027
                // If the texture data is embedded, the host application can then load 'embedded' texture data directly from the aiScene.mTextures array.
                var aiMaterialTextures = aiMaterial.GetAllMaterialTextures();
                Console.Write("\n aiMaterialTextures.Count: " + aiMaterialTextures.Count());
                for (int j = 0; j < aiMaterialTextures.Count(); j++) {
                    var aiTexture = aiMaterialTextures[j];                        
                    Console.Write("\n \n   " + "aiMaterialTexture [" + j + "]");
                    Console.Write("\n   " + "aiTexture.Name: " + ld.GetFileName(aiTexture.FilePath, true));
                    Console.Write("\n   " + "FilePath.: " + aiTexture.FilePath);
                    Console.Write("\n   " + "texture.TextureType: " + aiTexture.TextureType);
                    Console.Write("\n   " + "texture.Operation: " + aiTexture.Operation);
                    Console.Write("\n   " + "texture.BlendFactor: " + aiTexture.BlendFactor);
                    Console.Write("\n   " + "texture.Mapping: " + aiTexture.Mapping);
                    Console.Write("\n   " + "texture.WrapModeU: " + aiTexture.WrapModeU + " , V: " + aiTexture.WrapModeV);
                    Console.Write("\n   " + "texture.UVIndex: " + aiTexture.UVIndex);
                }
            }                
            Console.Write("\n \n ///////////////////////////////////////////////////");
            Console.Write("\n scene.Animations \n ");                
            var aiAnimations = scene.Animations;
            for (int i = 0; i < aiAnimations.Count; i++) {
                var aiAnimation = aiAnimations[i];
                Console.Write("\n --------------------------------------------------");
                Console.Write("\n aiAnimation " + i + " of " + (aiAnimations.Count - 1) + "");
                Console.Write("\n aiAnimation.Name: " + aiAnimation.Name);
                if (aiAnimation.NodeAnimationChannels.Count > 0)
                    Console.Write("\n  " + " Animated Nodes...");
                for (int j = 0; j < aiAnimation.NodeAnimationChannels.Count; j++) {
                    var nodeAnimLists = aiAnimation.NodeAnimationChannels[j];
                    Console.Write("\n  " + " aiAnimation.NodeAnimationChannels[" + j + "].NodeName: " + nodeAnimLists.NodeName);
                    Node nodeinfo;
                    if (GetAssimpTreeNode(scene.RootNode, nodeAnimLists.NodeName, out nodeinfo)) {
                        if (nodeinfo.MeshIndices.Count > 0) {
                            Console.Write("  " + " HasMeshes: " + nodeinfo.MeshIndices.Count);
                            foreach (var n in nodeinfo.MeshIndices) Console.Write("  " + " : " + scene.Meshes[n].Name);
                        }
                    }
                }
            }
            Console.Write("\n -------------------------------------------------- \n "); Console.Write("\n ///////////////////////////////////////////////////");
            Console.Write("\n scene  NodeHeirarchy");
            AssimpNodeHeirarchyDump(scene.RootNode, 0); // ASSIMP NODE HIEARCHY DUMP            
        }

        // ASSIMP NODE HEIRARCHY DUMP
        public void AssimpNodeHeirarchyDump(Assimp.Node node, int spaces)
        {
            string indent = "";
            for (int i = 0; i < spaces; i++) indent += "  ";
            Console.Write("\n" + indent + "  node.Name: " + node.Name + "          HasMeshes: " + node.HasMeshes + "    MeshCount: " + node.MeshCount + 
                "    node.ChildCount: " + node.ChildCount + "    MeshIndices.Count " + node.MeshIndices.Count);
            for (int j = 0; j < node.MeshIndices.Count; j++) {
                var meshIndice = node.MeshIndices[j];
                Console.Write("\n" + indent + " *meshIndice: " + meshIndice + "  meshIndice name: " + ld.scene.Meshes[meshIndice].Name);
            }                        
            for (int n = 0; n < node.Children.Count(); n++) { AssimpNodeHeirarchyDump(node.Children[n], spaces + 1); }  // recursive
        }

        // GET ASSIMP TREE NODE
        public bool GetAssimpTreeNode(Assimp.Node treenode, string name, out Assimp.Node node)
        {
            bool found = false;  node = null;
            if (treenode.Name == name) {
                found = true;
                node = treenode;
            } else { foreach (var n in treenode.Children) { found = GetAssimpTreeNode(n, name, out node); } }
            return found;
        }
        #endregion //console assimp output


        #region CONSOLE MODEL OUTPUT (mostly):

        public void DisplayInfo(SkinModel model, string filePath)
        {
            if (ld.LoadedModelInfo)
            {
                Console.Write("\n\n\n\n****************************************************");
                Console.WriteLine("\n\n@@@DisplayInfo \n \n");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                Console.WriteLine("Model");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine();
                Console.WriteLine("FileName");
                Console.WriteLine(ld.GetFileName(filePath, true)); Console.WriteLine();
                Console.WriteLine("Path:");
                Console.WriteLine(filePath); Console.WriteLine();
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                Console.WriteLine("Animations");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine("");
                for (int i = 0; i < ld.scene.Animations.Count; i++)
                {
                    var anim = ld.scene.Animations[i];
                    Console.WriteLine($"_____________________________________");
                    Console.WriteLine($"Anim #[{i}] Name: {anim.Name}");
                    Console.WriteLine($"_____________________________________");
                    Console.WriteLine($"  Duration: {anim.DurationInTicks} / {anim.TicksPerSecond} sec.   total duration in seconds: {anim.DurationInTicks / anim.TicksPerSecond}");
                    Console.WriteLine($"  Node Animation Channels: {anim.NodeAnimationChannelCount} ");
                    Console.WriteLine($"  Mesh Animation Channels: {anim.MeshAnimationChannelCount} ");
                    Console.WriteLine($"  Mesh Morph     Channels: {anim.MeshMorphAnimationChannelCount} ");
                }
                Console.WriteLine();
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                Console.WriteLine("Node Heirarchy");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine("");
                InfoModelNode(model, model.rootNodeOfTree, 0); Console.WriteLine("");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                Console.WriteLine("Meshes and Materials");
                Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine("");
                InfoForMeshMaterials(model, ld.scene); Console.WriteLine("");
            }
            if (ld.MinimalInfo || ld.LoadedModelInfo)
            {
                MinimalInfo(model, filePath);
            }
        }

        // INFO MODEL NODE
        public void InfoModelNode(SkinModel model, SkinModel.ModelNode n, int tabLevel)
        {
            string ntab = "";
            for (int i = 0; i < tabLevel; i++) ntab += "  ";
            string rtab = "\n" + ntab;
            string msg = "\n";
            msg += rtab + $"{n.name}  ";
            msg += rtab + $"|_children.Count: {n.children.Count} ";
            if (n.parent == null) msg += $"|_parent: IsRoot ";
            else msg += $"|_parent: " + n.parent.name;
            msg += rtab + $"|_hasARealBone: {n.hasRealBone} ";
            msg += rtab + $"|_isThisAMeshNode: {n.isMeshNode}";
            if (n.uniqueMeshBones.Count > 0)
            {
                msg += rtab + $"|_uniqueMeshBones.Count: {n.uniqueMeshBones.Count}  ";
                int i = 0;
                foreach (var bone in n.uniqueMeshBones)
                {
                    msg += rtab + $"|_node: {n.name}  lists  uniqueMeshBone[{i}] ...  meshIndex: {bone.meshIndex}  meshBoneIndex: {bone.boneIndex}   " + $"mesh[{bone.meshIndex}]bone[{bone.boneIndex}].Name: {model.meshes[bone.meshIndex].meshBones[bone.boneIndex].name}  " + $"in  mesh[{bone.meshIndex}].Name: {model.meshes[bone.meshIndex].Name}";
                    var nameToCompare = model.meshes[bone.meshIndex].meshBones[bone.boneIndex].name;
                    int j = 0;
                    foreach (var anim in model.animations)
                    {
                        int k = 0;
                        foreach (var animNode in anim.animatedNodes)
                        {
                            if (animNode.nodeName == nameToCompare)
                                msg += rtab + $"|^has corresponding Animation[{j}].Node[{k}].Name: {animNode.nodeName}";
                            k++;
                        }
                        j++;
                    }
                    i++;
                }
            }
            Console.WriteLine(msg);
            for (int i = 0; i < n.children.Count; i++) { InfoModelNode(model, n.children[i], tabLevel + 1); }
        }

        // INFO FOR ANIM DATA
        public void InfoForAnimData(Scene scene)
        {
            if (ld.ConsoleInfo)
            {
                string str = "\n\n AssimpSceneConsoleOutput ========= Animation Data========= \n\n"; Console.WriteLine(str);
            }
            for (int i = 0; i < scene.Animations.Count; i++)
            {
                var anim = scene.Animations[i];
                if (ld.ConsoleInfo)
                {
                    Console.WriteLine($"_________________________________");
                    Console.WriteLine($"Anim #[{i}] Name: {anim.Name}");
                    Console.WriteLine($"_________________________________");
                    Console.WriteLine($"  Duration: {anim.DurationInTicks} / {anim.TicksPerSecond} sec.   total duration in seconds: {anim.DurationInTicks / anim.TicksPerSecond}");
                    Console.WriteLine($"  HasMeshAnimations: {anim.HasMeshAnimations} ");
                    Console.WriteLine($"  Mesh Animation Channels: {anim.MeshAnimationChannelCount} ");
                }
                foreach (var chan in anim.MeshAnimationChannels)
                {
                    if (ld.ConsoleInfo)
                    {
                        Console.WriteLine($"  Channel MeshName {chan.MeshName}");    // the node name has to be used to tie this channel to the originally printed hierarchy.  BTW, node names must be unique.
                        Console.WriteLine($"    HasMeshKeys: {chan.HasMeshKeys}");   // access via chan.PositionKeys
                        Console.WriteLine($"    MeshKeyCount: {chan.MeshKeyCount}"); //                                                               
                    }
                }
                if (ld.ConsoleInfo) Console.WriteLine($"  Mesh Morph Channels: {anim.MeshMorphAnimationChannelCount} ");
                foreach (var chan in anim.MeshMorphAnimationChannels)
                {
                    if (ld.ConsoleInfo)
                    {
                        Console.WriteLine($"  Channel {chan.Name}");
                        Console.WriteLine($"    HasMeshMorphKeys: {chan.HasMeshMorphKeys}");
                        Console.WriteLine($"     MeshMorphKeyCount: {chan.MeshMorphKeyCount}");
                    }
                }
                if (ld.ConsoleInfo)
                {
                    Console.WriteLine($"  HasNodeAnimations: {anim.HasNodeAnimations} ");
                    Console.WriteLine($"   Node Channels: {anim.NodeAnimationChannelCount}");
                }
                foreach (var chan in anim.NodeAnimationChannels)
                {
                    if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                    {
                        Console.Write($"   Channel {chan.NodeName}".PadRight(35));                     // the node name has to be used to tie this channel to the originally printed hierarchy.  BTW, node names must be unique.
                        Console.Write($"     Position Keys: {chan.PositionKeyCount}".PadRight(25));    // access via chan.PositionKeys
                        Console.Write($"     Rotation Keys: {chan.RotationKeyCount}".PadRight(25));    // 
                        Console.WriteLine($"     Scaling  Keys: {chan.ScalingKeyCount}".PadRight(25)); // 
                    }
                }
                if (ld.ConsoleInfo) Console.WriteLine("\n \n Ok so this is all gonna go into our model class basically as is.");                
                foreach (var anode in anim.NodeAnimationChannels)
                {
                    if ((ld.ConsoleInfo && ld.AnimationKeysInfo) || ld.targetNodeName == anode.NodeName)
                    {
                        Console.WriteLine($"   Channel {anode.NodeName}\n   (time is in animation ticks it shouldn't exceed anim.DurationInTicks {anim.DurationInTicks} or total duration in seconds: {anim.DurationInTicks / anim.TicksPerSecond})");        // the node name has to be used to tie this channel to the originally printed hierarchy.  node names must be unique.
                        Console.WriteLine($"     Position Keys: {anode.PositionKeyCount}");       // access via chan.PositionKeys
                        for (int j = 0; j < anode.PositionKeys.Count; j++)
                        {
                            var key = anode.PositionKeys[j];
                            if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                                Console.WriteLine("       index[" + (j + "]").PadRight(5) + " Time: " + key.Time.ToString().PadRight(17) + " secs: " 
                                    + (key.Time / anim.TicksPerSecond).ToString() + "  Position: {" + key.Value.ToStringTrimed() + "}");
                        }
                        if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                            Console.WriteLine($"     Rotation Keys: {anode.RotationKeyCount}");       // 
                        for (int j = 0; j < anode.RotationKeys.Count; j++)
                        {
                            var key = anode.RotationKeys[j];
                            if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                                Console.WriteLine("       index[" + (j + "]").PadRight(5) + " Time: " + key.Time.ToString() + " secs: " 
                                    + (key.Time / anim.TicksPerSecond).ToString() + "  QRotation: {" + key.Value.ToStringTrimed() + "}");
                        }
                        if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                            Console.WriteLine($"     Scaling  Keys: {anode.ScalingKeyCount}");        // 
                        for (int j = 0; j < anode.ScalingKeys.Count; j++)
                        {
                            var key = anode.ScalingKeys[j];
                            if (ld.ConsoleInfo && ld.AnimationKeysInfo)
                                Console.WriteLine("       index[" + (j + "]").PadRight(5) + " Time: " + key.Time.ToString() + " secs: " 
                                    + (key.Time / anim.TicksPerSecond).ToString() + "  Scaling: {" + key.Value.ToStringTrimed() + "}");
                        }
                    }
                }
            }
        }
        // I N F O   F O R   M E S H   M A T E R I A L S
        public void InfoForMeshMaterials(SkinModel model, Scene scene)
        {
            Console.WriteLine("InfoForMaterials");
            Console.WriteLine("Each mesh has a listing of bones that apply to it; this is just a reference to the bone.");
            Console.WriteLine("Each mesh has a corresponding Offset matrix for that bone."); Console.WriteLine("Important.");
            Console.WriteLine("This means that offsets are not common across meshes but bones can be.");
            Console.WriteLine("ie: The same bone node may apply to different meshes but that same bone will have a different applicable offset per mesh.");
            Console.WriteLine("Each mesh also has a corresponding bone weight per mesh.");
            for (int amLoop = 0; amLoop < scene.Meshes.Count; amLoop++)
            {                    // loop through assimp meshes
                Mesh assimpMesh = scene.Meshes[amLoop];
                Console.WriteLine("\n" + "__________________________" +
                "\n" + "scene.Meshes[" + amLoop + "] " + assimpMesh.Name +
                "\n" + " FaceCount: " + assimpMesh.FaceCount +
                "\n" + " VertexCount: " + assimpMesh.VertexCount +
                "\n" + " Normals.Count: " + assimpMesh.Normals.Count +
                "\n" + " Bones.Count: " + assimpMesh.Bones.Count +
                "\n" + " MaterialIndex: " + assimpMesh.MaterialIndex +
                "\n" + " MorphMethod: " + assimpMesh.MorphMethod +
                "\n" + " HasMeshAnimationAttachments: " + assimpMesh.HasMeshAnimationAttachments);
                Console.WriteLine(" UVComponentCount.Length: " + assimpMesh.UVComponentCount.Length);
                for (int i = 0; i < assimpMesh.UVComponentCount.Length; i++)
                {
                    int val = assimpMesh.UVComponentCount[i];
                    if (val > 0) Console.WriteLine("   mesh.UVComponentCount[" + i + "] : int value: " + val);
                }
                Console.WriteLine(" TextureCoordinateChannels.Length:" + assimpMesh.TextureCoordinateChannels.Length);
                Console.WriteLine(" TextureCoordinateChannelCount:" + assimpMesh.TextureCoordinateChannelCount);
                for (int i = 0; i < assimpMesh.TextureCoordinateChannels.Length; i++)
                {
                    var channel = assimpMesh.TextureCoordinateChannels[i];
                    if (channel.Count > 0) Console.WriteLine("   mesh.TextureCoordinateChannels[" + i + "]  count " + channel.Count);
                    //for (int j = 0; j < channel.Count; j++) { // holds uvs and ? i think //Console.Write(" channel[" + j + "].Count: " + channel.Count); }
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n" + "__________________________");
            if (scene.HasTextures)
            {
                var texturescount = scene.TextureCount;
                var textures = scene.Textures;
                Console.WriteLine("\n  Embedded Textures " + " Count " + texturescount);
                for (int i = 0; i < textures.Count; i++)
                {
                    var name = textures[i];
                    Console.WriteLine("    Embedded Textures[" + i + "] " + name);
                }
            }
            else { Console.WriteLine("\n    Embedded Textures " + " None "); }
            Console.WriteLine("\n" + "__________________________");
            if (scene.HasMaterials)
            {
                Console.WriteLine("\n    Materials scene.MaterialCount " + scene.MaterialCount + "\n");
                for (int i = 0; i < scene.Materials.Count; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine("\n    " + "__________________________");
                    Console.WriteLine("    Material[" + i + "] ");
                    Console.WriteLine("    Material[" + i + "].Name " + scene.Materials[i].Name);
                    var m = scene.Materials[i];
                    if (m.HasName) { Console.Write("     Name: " + m.Name); }
                    var t = m.GetAllMaterialTextures();
                    Console.WriteLine("    GetAllMaterialTextures Length " + t.Length);
                    Console.WriteLine();
                    for (int j = 0; j < t.Length; j++)
                    {
                        var tindex = t[j].TextureIndex;
                        var toperation = t[j].Operation;
                        var ttype = t[j].TextureType.ToString();
                        var tfilepath = t[j].FilePath;
                        // J matches up to the texture coordinate channel uv count it looks like.
                        Console.WriteLine("    Texture[" + j + "] " + "   Index:" + tindex + "   Type: " + ttype + "   Operation: " + toperation + "   Filepath: " + tfilepath);
                    }
                    Console.WriteLine();
                    // added info
                    Console.WriteLine("    Material[" + i + "] " + "  HasColorAmbient " + m.HasColorAmbient + "  HasColorDiffuse " + m.HasColorDiffuse + "  HasColorSpecular " + m.HasColorSpecular);
                    Console.WriteLine("    Material[" + i + "] " + "  HasColorReflective " + m.HasColorReflective + "  HasColorEmissive " + m.HasColorEmissive + "  HasColorTransparent " + m.HasColorTransparent);
                    Console.WriteLine("    Material[" + i + "] " + "  ColorAmbient:" + m.ColorAmbient + "  ColorDiffuse: " + m.ColorDiffuse + "  ColorSpecular: " + m.ColorSpecular);
                    Console.WriteLine("    Material[" + i + "] " + "  ColorReflective:" + m.ColorReflective + "  ColorEmissive: " + m.ColorEmissive + "  ColorTransparent: " + m.ColorTransparent);
                    Console.WriteLine("    Material[" + i + "] " + "  HasOpacity: " + m.HasOpacity + "  Opacity: " + m.Opacity + "  HasShininess:" + m.HasShininess + "  Shininess:" + m.Shininess + "  HasReflectivity: " + m.HasReflectivity + "  Reflectivity " + scene.Materials[i].Reflectivity);
                    Console.WriteLine("    Material[" + i + "] " + "  HasBlendMode:" + m.HasBlendMode + "  BlendMode:" + m.BlendMode + "  HasShadingMode: " + m.HasShadingMode + "  ShadingMode:" + m.ShadingMode + "  HasBumpScaling: " + m.HasBumpScaling + "  HasTwoSided: " + m.HasTwoSided + "  IsTwoSided: " + m.IsTwoSided);
                    Console.WriteLine("    Material[" + i + "] " + "  HasTextureAmbient " + m.HasTextureAmbient + "  HasTextureDiffuse " + m.HasTextureDiffuse + "  HasTextureSpecular " + m.HasTextureSpecular);
                    Console.WriteLine("    Material[" + i + "] " + "  HasTextureNormal " + m.HasTextureNormal + "  HasTextureHeight " + m.HasTextureHeight + "  HasTextureDisplacement:" + m.HasTextureDisplacement + "  HasTextureLightMap " + m.HasTextureLightMap);
                    Console.WriteLine("    Material[" + i + "] " + "  HasTextureReflection:" + m.HasTextureReflection + "  HasTextureOpacity " + m.HasTextureOpacity + "  HasTextureEmissive:" + m.HasTextureEmissive);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("\n   No Materials Present. \n");
            }
            Console.WriteLine();
            Console.WriteLine("\n" + "__________________________");
            Console.WriteLine("Bones in meshes");
            for (int mindex = 0; mindex < model.meshes.Length; mindex++)
            {
                var rmMesh = model.meshes[mindex];
                Console.WriteLine(); Console.WriteLine("\n" + "__________________________");
                Console.WriteLine("Bones in mesh[" + mindex + "]   " + rmMesh.Name); Console.WriteLine();
                if (rmMesh.hasBones)
                {
                    var meshBones = rmMesh.meshBones;
                    Console.WriteLine(" meshBones.Length: " + meshBones.Length);
                    for (int meshBoneIndex = 0; meshBoneIndex < meshBones.Length; meshBoneIndex++)
                    {
                        var boneInMesh = meshBones[meshBoneIndex]; // ahhhh
                        var boneInMeshName = meshBones[meshBoneIndex].name;
                        string str = "   mesh[" + mindex + "].Name: " + rmMesh.Name + "   bone[" + meshBoneIndex + "].Name: " + boneInMeshName + "   assimpMeshBoneIndex: " + meshBoneIndex.ToString() + "   WeightCount: " + boneInMesh.numWeightedVerts; //str += "\n" + "   OffsetMatrix " + boneInMesh.OffsetMatrix;
                        Console.WriteLine(str);
                    }
                }
                Console.WriteLine();
            }
        }
        // M I N I M A L   I N F O 
        public void MinimalInfo(SkinModel model, string filePath)
        {
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine();
            Console.WriteLine($"Model");
            Console.WriteLine($"{ld.GetFileName(filePath, true)}  Loaded"); Console.WriteLine();
            Console.WriteLine("Model sceneRootNodeOfTree's Node Name:     " + model.rootNodeOfTree.name);
            Console.WriteLine("Model number of animaton: " + model.animations.Count);
            Console.WriteLine("Model number of meshes:   " + model.meshes.Length);
            for (int mmLoop = 0; mmLoop < model.meshes.Length; mmLoop++)
            {
                var rmMesh = model.meshes[mmLoop];
                Console.WriteLine("Model mesh #" + mmLoop + " of  " + model.meshes.Length + "   Name: " + rmMesh.Name + "   MaterialIndex: " + rmMesh.material_index + "  MaterialIndexName: " + rmMesh.material_name + "  Bones.Count " + model.meshes[mmLoop].meshBones.Count() + " ([0] is a generated bone to the mesh)");
                if (rmMesh.tex_diffuse != null)
                    Console.WriteLine("texture: " + rmMesh.tex_name);
                if (rmMesh.tex_normalMap != null)
                    Console.WriteLine("textureNormalMap: " + rmMesh.tex_normMap_name);
                /// May add more texture types later in which case we may want to update this for debugging if needed
                //if (rmMesh.textureHeightMap != null) Console.WriteLine("textureHeightMap: " + rmMesh.textureHeightMapName);
            }
            Console.WriteLine(); Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"); Console.WriteLine("\n");
        }

        #endregion


        #region ADDITIONAL REPORTS:

        // CREATING ROOT INFO
        public void CreatingRootInfo(string str1)
        {
            Console.WriteLine("\n\n@@@CreateRootNode \n");
            Console.WriteLine("\n\n Prep to build a models tree. Set Up the Models RootNode");
            Console.WriteLine(str1);
        }

        // SHOW MESH BONE CREATION INFO
        public void ShowMeshBoneCreationInfo(Mesh assimpMesh, SkinModel.SkinMesh sMesh, bool MatrixInfo, int mi)
        {
            // If an imported model uses multiple materials, the import splits up the mesh. Use this value as index into the scene's material list. 
            // http://sir-kimmi.de/assimp/lib_html/structai_mesh.html#aa2807c7ba172115203ed16047ad65f9e                   
            Console.Write("\n\n Name " + assimpMesh.Name + " scene.Mesh[" + mi + "] ");
            Console.Write("\n" + " assimpMesh.VertexCount: " + assimpMesh.VertexCount + "  rmMesh.MaterialIndexName: " + sMesh.material_name 
                        + "   Material index: " + sMesh.material_index + " (material associated to this mesh)  " + " Bones.Count: " + assimpMesh.Bones.Count);
            Console.Write("\n" + " Note bone 0 doesn't exist in the original assimp bone data structure to facilitate a bone 0 for mesh node transforms so " +
                          "that aibone[0] is converted to modelBone[1]");
            for (int i = 0; i < sMesh.meshBones.Length; i++) {
                var bone = sMesh.meshBones[i];
                Console.Write("\n Bone [" + i + "] Name " + bone.name + "  meshIndex: " + bone.meshIndex + " meshBoneIndex: " 
                            + bone.boneIndex + " numberOfAssociatedWeightedVertices: " + bone.numWeightedVerts);
                if (MatrixInfo)
                    Console.Write("\n  Offset: " + bone.offset_mtx);
            }
        }

        // SHOW NODE TREE INFO
        public void ShowNodeTreeInfo(int tabLevel, Node curAssimpNode, bool matrixInfo, SkinModel.ModelNode modelNode, SkinModel model, Scene scene)
        {
            string ntab = "";
            for (int i = 0; i < tabLevel; i++) ntab += "  ";
            string ntab2 = ntab + "    ";

            Console.WriteLine("\n\n@@@CreateModelNodeTreeTransformsRecursively \n \n ");            
            Console.Write("\n " + ntab + "  ModelNode Name: " + modelNode.name + "  curAssimpNode.Name: " + curAssimpNode.Name);
            if (curAssimpNode.MeshIndices.Count > 0) {
                Console.Write("\n " + ntab + "  |_This node has mesh references.  aiMeshCount: " + curAssimpNode.MeshCount + " Listed MeshIndices: ");
                for (int i = 0; i < curAssimpNode.MeshIndices.Count; i++) Console.Write(" , " + curAssimpNode.MeshIndices[i]);
                for (int i = 0; i < curAssimpNode.MeshIndices.Count; i++) {
                    var nodesmesh = model.meshes[curAssimpNode.MeshIndices[i]];
                    Console.Write("\n " + ntab + " " + " |_Is a mesh ... Mesh nodeRefContainingAnimatedTransform Set to node: " 
                                + nodesmesh.node_with_anim_trans.name + "  mesh: " + nodesmesh.Name);
                }
            }
            if (matrixInfo) Console.WriteLine("\n " + ntab2 + "|_curAssimpNode.Transform: " + curAssimpNode.Transform.SrtInfoToString(ntab2));
            for (int mIndex = 0; mIndex < scene.Meshes.Count; mIndex++) {
                SkinModel.ModelBone bone;
                int boneIndexInMesh = 0;
                if (GetBoneForMesh(model.meshes[mIndex], modelNode.name, out bone, out boneIndexInMesh)) {
                    var adjustedBoneIndexInMesh = boneIndexInMesh;
                    Console.Write("\n " + ntab + "  |_The node will be marked as having a real bone node along the bone route.");
                    if (modelNode.isMeshNode) Console.Write("\n " + ntab + "  |_The node is also a mesh node so this is maybe a node targeting a mesh transform with animations.");
                    Console.Write("\n " + ntab + "  |_Adding uniqueBone for Node: " + modelNode.name + " of Mesh[" + mIndex + " of " + scene.Meshes.Count + "].Name: " + scene.Meshes[mIndex].Name);
                    Console.Write("\n " + ntab + "  |_It's a Bone  in mesh #" + mIndex + "  aiBoneIndexInMesh: " + (boneIndexInMesh - 1) + " adjusted BoneIndexInMesh: " + adjustedBoneIndexInMesh);
                }
            }
        }
        private bool GetBoneForMesh(SkinModel.SkinMesh sMesh, string name, out SkinModel.ModelBone bone, out int boneIndexInMesh) {
            bool found = false; bone = null; boneIndexInMesh = 0;
            for (int j = 0; j < sMesh.meshBones.Length; j++) {  // loop thru the bones of the mesh
                if (sMesh.meshBones[j].name == name) {          // found a bone whose name matches 
                    found = true;
                    bone = sMesh.meshBones[j];                  // return the matching bone
                    boneIndexInMesh = j;                        // return the index into the bone-list of the mesh
                }
            }
            return found;
        }

        // SHOW MESH INFO
        public void ShowMeshInfo(Mesh mesh, int mi) { 
            Console.WriteLine(
            "\n" + "__________________________" +
            "\n" + "scene.Meshes[" + mi + "] " + mesh.Name +
            "\n" + " FaceCount: " + mesh.FaceCount +
            "\n" + " VertexCount: " + mesh.VertexCount +
            "\n" + " Normals.Count: " + mesh.Normals.Count +
            "\n" + " Bones.Count: " + mesh.Bones.Count +
            "\n" + " HasMeshAnimationAttachments: " + mesh.HasMeshAnimationAttachments + "\n  (note mesh animations maybe linked to a node animation off the main bone transform chain.)" +
            "\n" + " MorphMethod: " + mesh.MorphMethod +
            "\n" + " MaterialIndex: " + mesh.MaterialIndex +
            "\n" + " VertexColorChannels.Count: " + mesh.VertexColorChannels[mi].Count +
            "\n" + " Tangents.Count: " + mesh.Tangents.Count +
            "\n" + " BiTangents.Count: " + mesh.BiTangents.Count +
            "\n" + " UVComponentCount.Length: " + mesh.UVComponentCount.Length
            );
        }
    #endregion

    }
}
