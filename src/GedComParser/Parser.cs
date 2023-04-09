using MaiorumSeries.GedComModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MaiorumSeries.GedComParser
{
    /// <summary>
    /// This is the GEDCOM Parser Class
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Parse the file content as gedcom file format as generic model which is basically a 
        /// structure of key value pairs.
        /// </summary>
        /// <param name="fileName"></param>
        public BaseRecord ParseGenericModel (IParserContext context, string fileName)
        {

            context.ReadLines = 0;
            context.ProcessedLine = 0;

            BaseRecord modelRecord = new BaseRecord()
            {
                Tag = "Model"
            };

            using (StreamReader file = new StreamReader(fileName, true))
            {

                //context.WriteLine("The file {0} has Encoding: {1}", fileName, file.CurrentEncoding.ToString());

                context.Line = 0;
                string ln;
                Token lastToken = null;
                int currentLevel = -1;
                List<BaseRecord> records = new List<BaseRecord>();
                BaseRecord currentRecord = modelRecord;
                records.Add(currentRecord);

                while ((ln = file.ReadLine()) != null)
                {
                    var token = GetToken(ln, lastToken);
                    context.ReadLines++;
                    var newRecord = token.GetRecord();
                    //Console.WriteLine(token.ToString());

                    if (currentLevel + 1 == token.Level)
                    {
                        // New Level; stack new record
                        records.Add(newRecord);
                        if (records.Count > 1)
                        {
                            records[records.Count - 2].Records.AddBaseRecord(newRecord);
                        }
                    }
                    else if (currentLevel > token.Level)
                    {
                        // Back one level
                        while ((records.Count -1)  > token.Level)
                        {
                            records.RemoveAt(records.Count-1);
                        }
                        records.Add(newRecord);
                        if (records.Count > 1)
                        {
                            records[records.Count - 2].Records.AddBaseRecord(newRecord);
                        }
                    }
                    else if (currentLevel == token.Level)
                    {
                        records.RemoveAt(records.Count - 1);
                        records.Add(newRecord);
                        if (records.Count > 1)
                        {
                            records[records.Count - 2].Records.AddBaseRecord(newRecord);
                        }
                        // Same level
                    }
                    else
                    {
                        throw new ApplicationException($"Level has illigal value {token.Level} in line {context.Line}");
                    }
                    context.Line++;
                    lastToken = token;
                    currentLevel = token.Level;
                }
                file.Close();
            }
            return modelRecord;
        }


        /// <summary>
        /// Transform the generic base record into a typed record 
        /// </summary>
        /// <param name="context">Parser Context</param>
        /// <param name="mappingConfig">Mapping of all typed model classes mapped to keywords</param>
        /// <param name="baseRecord">Generic base record</param>
        /// <param name="typedBaseRecord">correlated typed record</param>
        /// <param name="type">The type of the typed record</param>
        private void TransformRecord(IParserContext context, MappingConfig mappingConfig, BaseRecord baseRecord, TypedBaseRecord typedBaseRecord, Type type)
        {
            // Get all properties from the target type
            var properties = type.GetProperties();
            foreach (var record in baseRecord.Records)
            {
                #region  First Search for the target Property by name and check some properties 

                PropertyInfo targetProperty = null;

                foreach (var p in properties)
                {
                    var attr = (GedComTagAttribute)p.GetCustomAttribute(typeof(GedComTagAttribute));
                    if (attr != null)
                    {
                        if (attr.Tags.Contains (record.Tag))
                        {
                            targetProperty = p;
                            break;
                        }
                        if ((!string.IsNullOrEmpty (baseRecord.Tag)) && 
                            (attr.Tags.Contains(baseRecord.Tag + "." + record.Tag)))
                        {
                            targetProperty = p;
                            break;
                        }
                    }
                }

                #endregion 

                if (targetProperty != null)
                {
                    bool processed = false;
                    // If the tag has a dedicated record, try to map it to the typed model object.

                    if (mappingConfig.ContainsKey(baseRecord.Tag, record.Tag))
                    {
                        // This record describes a domain data object, 
                        // Create it and transform it
                        TypedBaseRecord subTypedBaseRecord = (TypedBaseRecord)Activator.CreateInstance(mappingConfig.GetType(baseRecord.Tag, record.Tag));

                        subTypedBaseRecord.Value = record.Value;
                        subTypedBaseRecord.XrefId = record.XrefId;
                        subTypedBaseRecord.Tag = record.Tag;

                        // Set Properties when the are directly TypedBaseRecord (shall not happen) 
                        if (targetProperty.PropertyType == typeof(TypedBaseRecord))
                        {
                            targetProperty.SetValue(typedBaseRecord, subTypedBaseRecord);
                            processed = true;
                        }
                        // St property, when the type is derived from TypedBaseRecord
                        else if (targetProperty.PropertyType.BaseType != null && targetProperty.PropertyType.BaseType == typeof(TypedBaseRecord))
                        {
                            targetProperty.SetValue(typedBaseRecord, subTypedBaseRecord);
                            processed = true;
                        }
                        // Add record to a list, when the property is a list of derived types
                        else if (targetProperty.PropertyType.IsGenericType && targetProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            // Add the new record to a list, when it is a List of records
                            // Cast to IEnumerable<MyBaseClass> isn't helping you, so why bother?
                            object cln = targetProperty.GetValue(typedBaseRecord, null);

                            // Invoke Add method on 'cln', passing 'myBaseClassInstance' as the only argument.
                            targetProperty.PropertyType.GetMethod("Add").Invoke(cln, new[] { subTypedBaseRecord });
                            processed = true;
                        }
                        // If it handled as record, process it recursivly 
                        if (processed)
                        {
                            TransformRecord(context, mappingConfig, record, subTypedBaseRecord, mappingConfig.GetType(baseRecord.Tag, record.Tag));
                        }
                  
                    }
                    if (!processed)
                    {
                        // No matching record for the target property, try the internal data types 
                        if (targetProperty.PropertyType == typeof(string))
                        {
                            targetProperty.SetValue(typedBaseRecord, record.Value);
                            if (record.Records.Count > 0)
                            {
                                // Modeling error. There are sub records, while this is mapped to a simple string. 
                                // Something is wrong here with the typed model
                                context.WriteError(Errors.GCP1000.ToString(),  $"{context.Line} In the record {baseRecord.Tag} the mapped typed record {type.Name} is mapping to a property {targetProperty.Name} which does not allow to map the sub records");
                            }
                            processed = true;
                        }
                        // Add record to a list, when the property is a list of strings
                        else if (targetProperty.PropertyType.IsGenericType && targetProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            // Add the new record to a list, when it is a List of records
                            // Cast to IEnumerable<MyBaseClass> isn't helping you, so why bother?
                            object cln = targetProperty.GetValue(typedBaseRecord, null);

                            // Invoke Add method on 'cln', passing 'myBaseClassInstance' as the only argument.
                            targetProperty.PropertyType.GetMethod("Add").Invoke(cln, new[] { record.Value });
                            processed = true;
                        }
                        else
                        {
                            throw new ApplicationException($"Build the model detects a property {targetProperty.Name} with unknown data type {targetProperty.PropertyType} to handle");
                        }
                    }
                    if (processed)
                    {
                        context.ProcessedLine++;
                    }
                    else
                    {
                        context.WriteError(Errors.GCP1001.ToString(), 
                            $"Record {record.Tag} has unprocessed property {targetProperty.Name}");
                    }
                }
                else
                {
                    // Found tag in the file is not covered by the typed model due to missing tags
                    if (context.Debug)
                    {
                        context.WriteError(Errors.GCP1002.ToString(),
                           $"In the context {baseRecord.Tag} The Tag {record.Tag} is not covered in the model");
                    }
                    typedBaseRecord.Records.Add(record);
                }
            }
        }

        public Model TansformGenericModelToModel(IParserContext context, BaseRecord genericModel)
        {
            Model model = new Model();
            var mappingConfig = Model.GetRecordMappingConfig();


            TransformRecord(context, mappingConfig, genericModel, model, typeof (Model));
            return model;
        }

        public Token GetToken(string line, Token lastToken)
        {
            var token = new Token();

            int l;
            string work;

            int idx = 0;
            int startLevel = 0;
            int levelLength = 0;

            while (!char.IsDigit(line[idx]))
            {
                idx++;
                startLevel++;
            }
            while (char.IsDigit(line[idx]))
            {
                idx++;
                levelLength++;
            }

            work = line.Substring(startLevel, levelLength);
            l = int.Parse(work);
            token.Level = l;

            while (char.IsWhiteSpace(line[idx]))
            {
                idx++;
            }


            if (line[idx] == '@')
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append(line[idx]);
                idx++;
                while (line[idx] != '@')
                {
                    buffer.Append(line[idx++]);
                }
                buffer.Append(line[idx++]); // Add last @
                token.XrefId = buffer.ToString();

                while (idx < line.Length && char.IsWhiteSpace(line[idx]))
                {
                    idx++;
                }
            }
            else
            {
                token.XrefId = "";
            }

           

            {
                StringBuilder buffer = new StringBuilder();
                while (idx < line.Length &&  !char.IsWhiteSpace(line[idx]))
                {
                    buffer.Append(line[idx++]);
                }
                token.Tag = buffer.ToString();

                //// Very dirty hack for not unique tag to record mapping in GEDCOM
                //if ((lastToken?.Tag == "HEAD") && (token.Tag == "SOUR"))
                //{
                //    token.Tag = "HEAD.SOUR"; // This is the tag name in the attribute
                //}
            }

            while (idx < line.Length && char.IsWhiteSpace(line[idx]))
            {
                idx++;
            }

            StringBuilder data = new StringBuilder();
            while (idx < line.Length)
            {
                data.Append(line[idx++]);
            }
            token.Value = data.ToString();


            //if (line.Length > (idx))
            //{
            //    data = line.Substring(idx);
            //}
            //else
            //{
            //    data = "";
            //}

            //System.Console.WriteLine (token);

            return token;
        }

        public Model Parse (IParserContext context, string fileName)
        {

            var genericModel = ParseGenericModel(context, fileName);

            var model = TansformGenericModelToModel(context, genericModel);

            return model;
        }

    }
}
