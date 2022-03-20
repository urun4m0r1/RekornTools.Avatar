using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [CreateAssetMenu(menuName = "VRCAvatarTools/ShaderPropertyTables")]
    public class ShaderPropertyTable : ScriptableObject
    {
        [SerializeField] public ShaderPropertiesTables Tables = new ShaderPropertiesTables();

        public void OnValidate()
        {
            foreach (var table in Tables)
            {
                Shader shader = table.Key;
                var propertiesTypes = table.Value = new List<ShaderPropertiesType>();

                if (shader)
                {
                    int propertyCount = shader.GetPropertyCount();
                    if (propertyCount > 0)
                    {
                        foreach (ShaderPropertyType type in Enum.GetValues(typeof(ShaderPropertyType)))
                        {
                            var item = new ShaderPropertiesType
                            {
                                Key   = type,
                                Value = new ShaderProperties(),
                            };

                            propertiesTypes.Add(item);
                        }

                        for (var i = 0; i < propertyCount; i++)
                        {
                            var element = new ShaderProperty
                            {
                                Key   = i,
                                Value = shader.GetPropertyName(i),
                            };

                            foreach (ShaderPropertiesType propertiesType in propertiesTypes)
                            {
                                if (shader.GetPropertyType(i) == propertiesType.Key)
                                {
                                    propertiesType.Value.Add(element);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public class ShaderPropertiesTables : SerializedList<ShaderPropertiesTable> { }

    [Serializable]
    public class ShaderPropertiesTable : SerializedKeyValuePair<Shader, List<ShaderPropertiesType>> { }

    [Serializable]
    public class ShaderPropertiesTypes : List<ShaderPropertiesType> { }

    [Serializable]
    public class ShaderPropertiesType : SerializedKeyValuePair<ShaderPropertyType, ShaderProperties> { }

    [Serializable]
    public class ShaderProperties : SerializedList<ShaderProperty> { }

    [Serializable]
    public class ShaderProperty : SerializedKeyValuePair<int, string> { }
}
