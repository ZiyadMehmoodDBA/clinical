using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MDVision.Business.BCommon;
using MDVision.Datasets;
using System.Data;
using MDVision.DataAccess.DAL.Document;
using MDVision.Common.Shared;
using MDVision.Common.Logging;
using MDVision.Model.Lookups;
using MDVision.Model.Document;
namespace MDVision.Business.BLL
{
    public class BLLDocument
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BLLDocument"/> class.
        /// </summary>
        public BLLDocument()
        {
            //SharedVariable 
            //This call is required by the Web Services Designer.
            InitializeComponent();
            // this. = ;
            //Add your own initialization code after the InitializeComponent() call
        }

        private IContainer components;
        //NOTE: The following procedure is required by the Web Services Designer
        //It can be modified using the Web Services Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Variable

        #endregion

        #region Documents

        /// <summary>
        /// Loads the document.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="DocTypeId">The document type identifier.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public BLObject<DSDocument> LoadDocument(int DocId, string ShortName, int DocTypeId, string IsActive, string EntityId, string Description, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSDocument ds = new DSDocument();
                ds = new DALDocument().LoadDocument(DocId, ShortName, DocTypeId, IsActive, EntityId, Description, PageNumber, RowsPerPage);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LoadDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> InsertDocument(DSDocument ds)
        {
            try
            {
                ds = new DALDocument().InsertDocument(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::InsertDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> UpdateDocument(DSDocument ds)
        {
            try
            {
                ds = new DALDocument().UpdateDocument(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::UpdateDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Message.
        /// </summary>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteDocument(string DocId)
        {
            try
            {
                DocId = new DALDocument().DeleteDocument(DocId);
                return new BLObject<string>(DocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::DeleteDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region Documents Type


        /// <summary>
        /// Loads the type of the document.
        /// </summary>
        /// <param name="DocTypeId">The document type identifier.</param>
        /// <param name="ShortName">The short name.</param>
        /// <param name="EntityId">The entity identifier.</param>
        /// <returns></returns>
        public BLObject<DSDocument> LoadDocumentType(int DocTypeId, string ShortName, string Description, string IsActive, string EntityId, int PageNumber = 1, int RowsPerPage = 1000)
        {
            try
            {
                DSDocument dsDocumentType = new DSDocument();
                dsDocumentType = new DALDocument().LoadDocumentType(DocTypeId, ShortName, Description, IsActive, EntityId, PageNumber, RowsPerPage);
                return new BLObject<DSDocument>(dsDocumentType);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LoadDocumentType", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the document Type.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> InsertDocumentType(DSDocument ds)
        {
            try
            {
                ds = new DALDocument().InsertDocumentType(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::InsertDocumentType", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the document Type.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> UpdateDocumentType(DSDocument ds)
        {
            try
            {
                ds = new DALDocument().UpdateDocumentType(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::UpdateDocumentType", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }


        /// <summary>
        /// Deletes the type of the document.
        /// </summary>
        /// <param name="DocTypeId">The document type identifier.</param>
        /// <returns></returns>
        public BLObject<string> DeleteDocumentType(string DocTypeId)
        {
            try
            {
                DocTypeId = new DALDocument().DeleteDocumentType(DocTypeId);
                return new BLObject<string>(DocTypeId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::DeleteDocumentType", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public List<DocumentActivity> LoadDocumentsActivity(Int64 DocID, int PageNumber, int RowsPerPage)
        {

            return new DALDocument().LoadDocumentActivity(DocID, PageNumber, RowsPerPage);
        }
        public List<DocumentTag> GetDocumentsTag(Int64 TagID, int PageNumber, int RowsPerPage)
        {

            return new DALDocument().GetDocumentTags(TagID, PageNumber, RowsPerPage);
        }
        public List<DocumentTag> GetTagDocumentsByName(string name)
        {
            return new DALDocument().GetTagDocumentsByName(name);
        }
        public string AddNewtagDocument(DocumentTag model)
        {
            try
            {

                string lResult = new DALDocument().InsertNewTagDocument(model);
                return lResult;
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::AddNewtagDocument", ex);
                return string.Empty;
            }
        }
        public BLObject<string> DeleteTagDocuments(string TagID)
        {
            try
            {

                TagID = new DALDocument().DeleteTagDocuments(TagID);
                return new BLObject<string>(TagID);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::DeleteTagDocuments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }

        public BLObject<string> ActiveInActiveTagDocuments(DocumentTag model)
        {
            try
            {
                string lResult = string.Empty;
                lResult = new DALDocument().ActiveInActiveTagDocuments(model);
                return new BLObject<string>(lResult);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::ActiveInActiveTagDocuments", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region Medical Documents

        /// <summary>
        /// Loads the Medical document.
        /// </summary>
        /// <param name="DocId">The document identifier.</param>

        /// <returns></returns>
        public BLObject<DSDocument> LoadMedicalDocument(int MedicalDocId)
        {
            try
            {
                DSDocument ds = new DSDocument();
                ds = new DALMedicalDocument().LoadMedicalDocument(MedicalDocId);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LoadMedicalDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }


        /// <summary>
        /// Inserts the Medical document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> InsertMedicalDocument(DSDocument ds)
        {
            try
            {
                ds = new DALMedicalDocument().InsertMedicalDocument(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::InsertMedicalDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }

        /// <summary>
        /// Updates the Medical document.
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <returns></returns>
        public BLObject<DSDocument> UpdateMedicalDocument(DSDocument ds)
        {
            try
            {
                ds = new DALMedicalDocument().UpdateMedicalDocument(ds);
                return new BLObject<DSDocument>(ds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::UpdateMedicalDocument", ex);
                return new BLObject<DSDocument>(null, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the patient Medical Document.
        /// </summary>
        /// <param name="PatMsgId">The patient Message identifier.</param>
        /// <returns>BLObject&lt;System.String&gt;.</returns>
        public BLObject<string> DeleteMedicalDocument(string MedicalDocId)
        {
            try
            {
                MedicalDocId = new DALMedicalDocument().DeleteMedicalDocument(MedicalDocId);
                return new BLObject<string>(MedicalDocId);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::DeleteMedicalDocument", ex);
                return new BLObject<string>(null, ex.Message);
            }
        }
        #endregion

        #region "Lookups"
        /// <summary>
        /// Lookups the  document.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSDocumentLookup> LookupDocument(string PatientId)
        {
            try
            {
                DSDocumentLookup ds = new DSDocumentLookup();
                ds = new DALDocument().LookupDocument(PatientId);

                return new BLObject<DSDocumentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LookupDocument", ex);
                return new BLObject<DSDocumentLookup>(null, ex.Message);
            }

        }


        public BLObject<DSDocumentLookup> GetPatientDocument(Int64 PatientId,DateTime? FromDOS=null,DateTime? ToDOS=null,DateTime? FromEntry=null,DateTime? ToEntry=null,
            string AccountNumber=null,int AssignedToReviewedID=0,string EnteredBy=null,string DocPriority=null, DateTime? FromExpiry =null, DateTime? ToExpiry=null,
            long tagId=0,bool IsrecentCheck=false)
        {
            try
            {
                DSDocumentLookup ds = new DSDocumentLookup();
                ds = new DALDocument().GetPatientDocument(PatientId,FromDOS,ToDOS,FromEntry,ToEntry,AccountNumber,AssignedToReviewedID,EnteredBy,DocPriority,FromExpiry
                    ,ToExpiry,tagId,IsrecentCheck);

                return new BLObject<DSDocumentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::GetPatientDocument", ex);
                return new BLObject<DSDocumentLookup>(null, ex.Message);
            }

        }
        /// <summary>
        /// Lookups the  Folder.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSDocumentLookup> LookupFolders(string PatientId)
        {
            try
            {
                DSDocumentLookup ds = new DSDocumentLookup();
                ds = new DALDocument().LookupFolders(PatientId);

                return new BLObject<DSDocumentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LookupFolders", ex);
                return new BLObject<DSDocumentLookup>(null, ex.Message);
            }

        }

        /// <summary>
        /// Lookups the type of the document.
        /// </summary>
        /// <returns></returns>
        public BLObject<DSDocumentLookup> LookupDocumentType()
        {
            try
            {
                DSDocumentLookup ds = new DSDocumentLookup();
                ds = new DALDocument().LookupDocumentType();

                return new BLObject<DSDocumentLookup>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LookupDocumentType", ex);
                return new BLObject<DSDocumentLookup>(null, ex.Message);
            }

        }

        public BLObject<List<PatientRepresentativeLookupModel>> LookupDocumentProvider(long PatientId)
        {
            try
            {
                var obj = new DALDocument().LookupDocumentProvider(PatientId);
                return new BLObject<List<PatientRepresentativeLookupModel>>(obj);


            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLDocument::LookupDocumentProvider", ex);
                return new BLObject<List<PatientRepresentativeLookupModel>>(null, ex.Message);
            }

        }
        #endregion
    }
}
