using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.DataAccess.DAL.Batch;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Model.Batch.HL7ImmunizationBatch;
using System;
using System.Collections.Generic;
using MDVision.Datasets;

namespace MDVision.Business.BLL
{
    public class BLLBatch
    {
        public BLObject<List<HL7ImmunizationBatchModel>> LoadHL7ImmunizationBatch(HL7ImmunizationBatchModel mdl)
        {
            try
            {
                var result = new DALBatch().LoadHL7ImmunizationBatch(mdl);
                return new BLObject<List<HL7ImmunizationBatchModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::LoadHL7ImmunizationBatch", ex);
                return new BLObject<List<HL7ImmunizationBatchModel>>(null, ex.Message);
            }
        }
        public BLObject<List<HL7ImmunizationBatchModel>> LoadHL7ImmunizationQueue(HL7ImmunizationBatchModel mdl)
        {
            try
            {
                var result = new DALBatch().LoadHL7ImmunizationQueue(mdl);
                return new BLObject<List<HL7ImmunizationBatchModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::LoadHL7ImmunizationQueue", ex);
                return new BLObject<List<HL7ImmunizationBatchModel>>(null, ex.Message);
            }
        }
        public BLObject<string> DeleteHL7ImmunizationBatch(string BatchIds)
        {
            var result = "";
            try
            {
                result = new DALBatch().DeleteHL7ImmunizationBatch(BatchIds);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::DeleteHL7ImmunizationBatch", ex);
                return new BLObject<string>(null, ex.Message);

            }
        }
        public BLObject<string> MarkBatchAsCompleted(string BatchIds)
        {
            var result = "";
            try
            {
                result = new DALBatch().MarkBatchAsCompleted(BatchIds);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::MarkBatchAsCompleted", ex);
                return new BLObject<string>(null, ex.Message);

            }
        }
        public BLObject<string> ReProcessBatch(string BatchIds)
        {
            var result = "";
            try
            {
                result = new DALBatch().ReProcessBatch(BatchIds);
                return new BLObject<string>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::ReProcessBatch", ex);
                return new BLObject<string>(null, ex.Message);

            }
        }
        public BLObject<DSImmunizationHL7> LookupHL7BatchMessageType()
        {
            try
            {
                DSImmunizationHL7 ds = new DSImmunizationHL7();
                ds = new DALBatch().LookupHL7BatchMessageType();
                return new BLObject<DSImmunizationHL7>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::LookupHL7MessageType", ex);
                return new BLObject<DSImmunizationHL7>(null, ex.Message);
            }
        }

        public BLObject<DSImmunizationHL7> LookupHL7BatchStatus()
        {
            try
            {
                DSImmunizationHL7 ds = new DSImmunizationHL7();
                ds = new DALBatch().LookupHL7BatchStatus();
                return new BLObject<DSImmunizationHL7>(ds);

            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::LookupHL7BatchStatus", ex);
                return new BLObject<DSImmunizationHL7>(null, ex.Message);
            }
        }
        public BLObject<List<HL7ImmunizationBatchModel>> LoadHL7ImmunizationBatchById(HL7ImmunizationBatchModel mdl)
        {
            try
            {
                var result = new DALBatch().LoadHL7ImmunizationBatchById(mdl);
                return new BLObject<List<HL7ImmunizationBatchModel>>(result);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLBatch::LoadHL7ImmunizationBatchById", ex);
                return new BLObject<List<HL7ImmunizationBatchModel>>(null, ex.Message);
            }
        }
    }
}
