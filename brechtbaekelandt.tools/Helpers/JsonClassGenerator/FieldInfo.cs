﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace brechtbaekelandt.tools.Helpers.JsonClassGenerator
{
    [DebuggerDisplay("MemberName = {MemberName}/{JsonMemberName}")]
    public class FieldInfo
    {
        public FieldInfo(IJsonClassGeneratorConfig generator, string jsonMemberName, JsonType type, bool usePascalCase)
        {
            this.generator = generator;
            this.JsonMemberName = jsonMemberName;
            this.MemberName = jsonMemberName;
            if (usePascalCase) this.MemberName = JsonClassGenerator.ToTitleCase(this.MemberName);
            this.Type = type;
        }

        private IJsonClassGeneratorConfig generator;
        public string MemberName { get; private set; }
        public string JsonMemberName { get; private set; }
        public JsonType Type { get; private set; }

        public void UpdateMemberName(string newMemberName)
        {
            MemberName = newMemberName;
            JsonMemberName = newMemberName;
        }

        public string GetGenerationCode(string jobject)
        {
            var field = this;

            switch (field.Type.Type)
            {
                case JsonTypeEnum.Array:
                    var innermost = field.Type.GetInnermostType();
                    return string.Format("({1})JsonClassHelper.ReadArray<{5}>(JsonClassHelper.GetJToken<JArray>({0}, \"{2}\"), JsonClassHelper.{3}, typeof({6}))",
                        jobject,
                        field.Type.GetTypeName(),
                        field.JsonMemberName,
                        innermost.GetReaderName(),
                        -1,
                        innermost.GetTypeName(),
                        field.Type.GetTypeName()
                    );
                case JsonTypeEnum.Dictionary:
                    return string.Format("({1})JsonClassHelper.ReadDictionary<{2}>(JsonClassHelper.GetJToken<JObject>({0}, \"{3}\"))",
                        jobject,
                        field.Type.GetTypeName(),
                        field.Type.InternalType.GetTypeName(),
                        field.JsonMemberName,
                        field.Type.GetTypeName()
                    );
                default:
                    return string.Format("JsonClassHelper.{1}(JsonClassHelper.GetJToken<{2}>({0}, \"{3}\"))",
                        jobject,
                        field.Type.GetReaderName(),
                        field.Type.GetJTokenType(),
                        field.JsonMemberName);
            }
        }
    }
}
