using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderPropertiesTables : SerializedList<ShaderPropertiesTable> { }

    [Serializable]
    public class ShaderPropertiesTable : SerializedKeyValuePair<Shader, ShaderPropertyList> { }

    [Serializable]
    public class ShaderPropertyList : ImmutableSerializedList<ShaderProperty> { }

    [CreateAssetMenu(menuName = "VRCAvatarTools/ShaderPropertyTables")]
    public class ShaderPropertyTable : ScriptableObject
    {
        [SerializeField] public ShaderPropertiesTables Tables = new ShaderPropertiesTables();
        [SerializeField] public ShaderPropertiesTable  Table  = new ShaderPropertiesTable();

        private Shader _prevShader1;
        private Shader _prevShader2;

        public void OnValidate()
        {
            Shader shader1 = Tables[0].Key;
            if (_prevShader1 != shader1)
            {
                _prevShader1 = shader1;
                UpdateShaders(Tables[0]);
            }

            Shader shader2 = Table.Key;
            if (_prevShader2 != shader2)
            {
                _prevShader2 = shader2;
                UpdateShaders(Table);
            }
        }

        public void UpdateShaders(ShaderPropertiesTable table)
        {
            Shader             shader          = table.Key;
            ShaderPropertyList propertiesTypes = table.Value;

            propertiesTypes.Clear();

            if (shader)
            {
                int propertyCount = shader.GetPropertyCount();
                if (propertyCount > 0)
                {
                    for (var i = 0; i < propertyCount; i++)
                    {
                        if (shader.GetPropertyFlags(i) == ShaderPropertyFlags.HideInInspector)
                            continue;

                        if (shader.GetPropertyType(i) != ShaderPropertyType.Texture)
                            continue;

                        var item = new ShaderProperty(shader, i, shader.GetPropertyName(i), shader.GetPropertyType(i));
                        propertiesTypes.Add(item);
                    }
                }
            }
        }
    }
}
