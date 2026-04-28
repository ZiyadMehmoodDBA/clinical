namespace EDIParser.Professional
{
    using EDIParser;
    using EDIParser.EDI837Segment;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using MDVision.Common.Utilities;
    public class EDI837Parser
    {
        private IContainer components;
        private EDIParser.DSHCFA._837ClaimRow CurrentClaim;
        private EDIParser.DSHCFA._837NameRow CurrentPatient;
        private EDIParser.DSHCFA._837NameRow CurrentPayer;
        private EDIParser.DSHCFA._837NameRow CurrentPayToAddressProvider;
        private EDIParser.DSHCFA._837NameRow CurrentProvider;
        private EDIParser.DSHCFA._837NameRow CurrentReferringProvider;
        private EDIParser.DSHCFA._837NameRow CurrentRenderingProvider;
        private EDIParser.DSHCFA._837NameRow CurrentServiceLocation;
        private EDIParser.DSHCFA._837NameRow CurrentSubscriber;
        private EDIParser.DSHCFA._837NameRow CurrentSupervisingProvider;
        private long CurrentVisitId;
        private long CurrentVisitPriority;
        private EDIParser.DSHCFA DSHCFA = new EDIParser.DSHCFA();
        private EDIParser.DSHCFA._837NameRow PreviousPatient;
        private EDIParser.DSHCFA._837NameRow PreviousProvider;
        private EDIParser.DSHCFA._837NameRow PreviousSubscriber;
        private long PreviousVisitId;
        public Dictionary<long, string> EroredClaims = new Dictionary<long, string>();

        public EDI837Parser(ref EDIParser.DSHCFA ds)
        {
            this.InitializeComponent();
            this.DSHCFA = ds;
        }

        private void CopyRow(ref EDIParser.DSHCFA._837NameRow PreviousRow, ref EDIParser.DSHCFA._837NameRow CurrentRow)
        {
            try
            {
                int count = this.DSHCFA._837Name.Columns.Count;
                if (CurrentRow != null)
                {
                    int num;
                    if (CurrentRow.RowState == DataRowState.Deleted)
                    {
                        for (num = 0; num <= (count - 1); num++)
                        {
                            PreviousRow[num] = string.Empty;
                        }
                    }
                    else
                    {
                        for (num = 0; num <= (count - 1); num++)
                        {
                            PreviousRow[num] = CurrentRow[num];
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void DeleteRow(long VisitId)
        {
            try
            {
                int num;
                
                for (num = this.DSHCFA._837SVDServiceLine.Rows.Count - 1; num >= 0; num--)
                {
                    EDIParser.DSHCFA._837SVDServiceLineRow row2 = (EDIParser.DSHCFA._837SVDServiceLineRow)this.DSHCFA._837SVDServiceLine.Rows[num];
                    if ((row2.RowState != DataRowState.Deleted) && (Convert.ToInt64(row2.VisitId) == VisitId))
                    {
                        this.DSHCFA._837SVDServiceLine.Rows[num].Delete();
                    }
                }
                
                this.DSHCFA._837Claim.AcceptChanges();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void FillDataForCurrentClaim()
        {
            try
            {
                this.CurrentProvider = this.GetProvider(this.CurrentVisitId);
                this.CurrentPayToAddressProvider = this.GetPayToAddressProvider(this.CurrentVisitId);
                this.CurrentClaim = this.GetClaim(this.CurrentVisitId);
                if (this.CurrentProvider == null)
                {
                    throw new Exception("Provider's Data is not filled properly");
                }
                if (this.CurrentSubscriber == null)
                {
                    throw new Exception("Subscriber's Data is not filled properly");
                }
                if (this.CurrentPayer == null)
                {
                    throw new Exception("Payer's Data is not filled properly");
                }
                if (this.CurrentClaim == null)
                {
                    throw new Exception("Claim's Data is not filled properly");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void FindNextClaimId()
        {
            long visitId = -1L;
            try
            {
                EDIParser.DSHCFA._837NameRow subscriber;
                EDIParser.DSHCFA._837NameRow provider;
                int num2;
                EDIParser.DSHCFA._837NameRow row = null;
                bool flag = false;
                long Priority = -1L;
               
                if (!flag)
                {
                    for (num2 = this.DSHCFA._837Name.Rows.Count - 1; num2 >= 0; num2--)
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[num2];
                        if (row.RowState != DataRowState.Deleted)
                        {
                            visitId = Convert.ToInt64(row.VisitId);
                            Priority = Convert.ToInt64(row.OriginalPriority);
                            subscriber = this.GetSubscriber(visitId, Priority);
                            provider = this.GetProvider(visitId);
                            if (this.IsEqual(subscriber, this.PreviousSubscriber) && this.IsEqual(provider, this.PreviousProvider))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                if (!flag)
                {
                    for (num2 = this.DSHCFA._837Name.Rows.Count - 1; num2 >= 0; num2--)
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[num2];
                        if (row.RowState != DataRowState.Deleted)
                        {
                            visitId = Convert.ToInt64(row.VisitId);
                            Priority = Convert.ToInt64(row.OriginalPriority);
                            provider = this.GetProvider(visitId);
                            if (this.IsEqual(provider, this.PreviousProvider))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
               
                if (flag)
                {
                    bool flag2 = false;
                    long num3 = visitId;
                    long num4 = Priority;
                    EDIParser.DSHCFA._837NameRow row5 = this.GetSubscriber(num3, num4);
                    EDIParser.DSHCFA._837NameRow row6 = this.GetProvider(num3);
                    for (num2 = this.DSHCFA._837Name.Rows.Count - 1; num2 >= 0; num2--)
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[num2];
                        if (row.RowState != DataRowState.Deleted)
                        {
                            visitId = Convert.ToInt64(row.VisitId);
                            Priority = Convert.ToInt64(row.OriginalPriority);
                            subscriber = this.GetSubscriber(visitId, Priority);
                            provider = this.GetProvider(visitId);
                            if ((this.IsEqual(subscriber, row5) && this.IsEqual(provider, row6)) && (subscriber.SBR02.ToString().Trim() == "18"))
                            {
                                flag2 = true;
                                break;
                            }
                        }
                    }
                    if (!flag2)
                    {
                        visitId = num3;
                        Priority = num4;
                    }
                }
                this.PreviousVisitId = this.CurrentVisitId;
                if (flag)
                {
                    this.CurrentVisitId = visitId;
                    this.CurrentVisitPriority = Priority;
                }
                else
                {
                    this.CurrentVisitId = -1L;
                    this.CurrentVisitPriority = -1L;
                }
            }
            catch (Exception exception)
            {
                this.CurrentVisitId = visitId;
                throw exception;
            }
        }

        public string Get837()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                EDICommon.SegCount = 0;
                EDICommon.HLCount = 1;
                builder.Append(this.GetHeader());
                builder.Append(this.GetLoops());
                builder.Append(this.GetTrailerSegments());
                str = builder.ToString().ToUpper();
                this.setSegmentCount(ref str);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public void setSegmentCount(ref string str)
        {
            try
            {
                string[] temp = str.Split('~');
                int count = temp.Length - 5; // remove 4 segments ISA,GS,GE,IEA and 1 more for empty item at last.
                string str_SE = string.Empty;

                foreach (var item in temp)
                    if (item.StartsWith("SE*"))
                        str_SE = item;

                string[] str_temp = str_SE.Split('*');
                if (str_temp.Length > 1)
                    str_temp[1] = count.ToString();

                string str_replace = string.Empty;

                foreach (var item in str_temp)
                {
                    var item_ = item + "*";
                    str_replace += item_;
                }

                str_replace = str_replace.Remove(str_replace.LastIndexOf("*"));
                str = str.Replace(str_SE, str_replace);

            }
            catch (Exception)
            {

            }
        }

        private string GetBillingProviderSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.HL_L1());
                Name name = new Name(this.CurrentProvider);
                if (!string.IsNullOrEmpty(this.CurrentProvider.PRV03))
                {
                    builder.Append(name.PRV);
                }
                builder.Append(name.NM1);
                builder.Append(name.N3);
                builder.Append(name.N4);
                if (this.CurrentProvider.REF02.Equals(""))
                {
                    throw new Exception("Claim# " + this.CurrentClaim.CLM01 + " Billing Provider SSN is missing");
                }
                builder.Append(name.REF);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private EDIParser.DSHCFA._837ClaimRow GetClaim(long VisitId)
        {
            EDIParser.DSHCFA._837ClaimRow row2;
            try
            {
                EDIParser.DSHCFA._837ClaimRow row = null;
                for (int i = this.DSHCFA._837Claim.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837ClaimRow)this.DSHCFA._837Claim.Rows[i];
                    if (Convert.ToInt64(row.VisitId) == VisitId)
                    {
                        row = (EDIParser.DSHCFA._837ClaimRow)this.DSHCFA._837Claim.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetClaimSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                EDIParser.DSHCFA._837ServiceLineRow[] rowService = (EDIParser.DSHCFA._837ServiceLineRow[])this.DSHCFA._837ServiceLine.Select(this.DSHCFA._837ServiceLine.VisitIdColumn.ColumnName + " = '" + this.CurrentVisitId.ToString() + "'", this.DSHCFA._837ServiceLine.CHARGE_ORDERColumn.ColumnName);
                Claim claim = new Claim(this.CurrentClaim, rowService, this.DSHCFA.VisitICDs);
                builder.Append(claim.CLM);
                if (this.CurrentClaim.DTP03_F.ToString() != "")
                {
                    builder.Append(claim.DTP_F);
                }
                if (this.CurrentClaim.DTP03_A.ToString() != "")
                {
                    builder.Append(claim.DTP_A);
                }
                if (this.CurrentClaim.DTP03_B.ToString() != "")
                {
                    builder.Append(claim.DTP_B);
                }
                if (this.CurrentClaim.DTP03_C.ToString() != "")
                {
                    builder.Append(claim.DTP_C);
                }
                if (this.CurrentClaim.DTP03_D.ToString() != "")
                {
                    builder.Append(claim.DTP_D);
                }
                if (this.CurrentClaim.DTP03_E.ToString() != "")
                {
                    builder.Append(claim.DTP_E);
                }
                if (this.CurrentClaim.PWK01.ToString() != "")
                {
                    builder.Append(claim.PWK);
                }
                if (this.CurrentClaim.REF02_PRIOR_AUTH.ToString() != "")
                {
                    builder.Append(claim.REF_PRIOR_AUTH);
                }
                if (!string.IsNullOrEmpty(this.CurrentClaim.REF02_PayerClmCtrlNo.ToString()) && !string.IsNullOrEmpty(this.CurrentClaim.CLM05_3.ToString()) && this.CurrentClaim.CLM05_3.ToString() != "1")
                {
                    builder.Append(claim.REF_PAYER_CLAIM_CtrlNo);
                }
                if (this.CurrentClaim.REF02_CLIA.ToString() != "")
                {
                    builder.Append(claim.REF_CLIA);
                }
                if (this.CurrentClaim.NTE02.ToString() != "" || this.CurrentClaim.NTE03.ToString() != "")
                {
                    builder.Append(claim.NTE);
                }
                builder.Append(claim.DX);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetHeader()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Header header = new Header((EDIParser.DSHCFA._837HeaderRow)this.DSHCFA._837Header.Rows[0]);
                builder.Append(header.ISA);
                builder.Append(header.GS);
                builder.Append(header.ST);
                builder.Append(header.BHT);
                builder.Append(header.NM1_Sub);
                builder.Append(header.PER_Sub);
                builder.Append(header.NM1_Rec);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                EDILogger.LogErrorMessage("EDI837Parser::GetHeader", exception);
                throw exception;
            }
            return str;
        }

        private string GetLoops()
        {
            string str = string.Empty;
            try
            {
                //StringBuilder builder = new StringBuilder();
                this.CurrentProvider = null;
                this.CurrentSubscriber = null;
                this.CurrentPatient = null;
                this.CurrentClaim = null;
                this.CurrentVisitId = -1L;
                this.PreviousProvider = null;
                this.PreviousSubscriber = null;
                this.PreviousPatient = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.NewRow();
                this.PreviousVisitId = -1L;
                // this.FindNextClaimId();
                long lenght_ = this.DSHCFA._837Claim.Rows.Count;
                if (lenght_ > 0)
                {
                    for (int i = 0; i < lenght_; i++)
                    {
                        try
                        {
                            StringBuilder builder = new StringBuilder();
                            this.FindNextClaimId();
                            this.CurrentPayer = null;
                            this.CurrentReferringProvider = null;
                            this.CurrentRenderingProvider = null;
                            this.CurrentPayToAddressProvider = null;
                            this.CurrentSupervisingProvider = null;
                            this.CurrentServiceLocation = null;
                            this.FillDataForCurrentClaim();

                          
                        
                            if (!this.IsEqual(this.PreviousSubscriber, this.CurrentSubscriber))
                            {
                                builder.Append(this.GetSubscriberSegments());
                                builder.Append(this.GetPayerSegments());
                            }
                            if (((this.CurrentSubscriber.SBR02.ToString().Trim() != "18") && (this.CurrentSubscriber.SBR02.ToString().Trim() != "")) && !this.IsEqual(this.PreviousPatient, this.CurrentPatient))
                            {
                                builder.Append(this.GetPatientSegments());
                            }
                            builder.Append(this.GetClaimSegments());
                            if (this.CurrentReferringProvider != null)
                            {
                                builder.Append(this.GetReferringProviderSegments());
                            }
                            if (this.CurrentRenderingProvider != null)
                            {
                                builder.Append(this.GetRenderingProviderSegments());
                            }
                            if (this.CurrentServiceLocation != null)
                            {
                                builder.Append(this.GetServiceLocationSegments());
                            }
                            if (this.CurrentSupervisingProvider != null)
                            {
                                builder.Append(this.GetSupervisingProviderSegments());
                            }
                            builder.Append(this.GetOthersSubscriberSegments());
                            builder.Append(this.GetServiceLineSegments());
                          
                            this.CopyRow(ref this.PreviousPatient, ref this.CurrentPatient);
                            if (this.CurrentVisitId != -1L)
                            {
                                this.DeleteRow(this.CurrentVisitId);
                            }
                            //this.FindNextClaimId();

                            str += builder.ToString();
                        }
                        catch (Exception ex)
                        {
                            this.EroredClaims.Add(this.CurrentVisitId, ex.Message);

                            if (this.CurrentVisitId != -1L)
                            {
                                this.DeleteRow(this.CurrentVisitId);
                            }

                            this.CurrentClaim = null;
                            this.CurrentPatient = null;
                            this.CurrentPayer = null;
                            this.CurrentPayToAddressProvider = null;
                            this.CurrentProvider = null;
                            this.CurrentReferringProvider = null;
                            this.CurrentRenderingProvider = null;
                            this.CurrentServiceLocation = null;
                            this.CurrentSubscriber = null;
                            this.CurrentSupervisingProvider = null;
                            this.CurrentVisitId = -1L;
                            this.CurrentVisitPriority = -1L;
                        }
                    }
                }
                else
                {
                    throw new Exception("Claim's Data is not filled properly");
                }

                //if (this.CurrentVisitId != -1L)
                //{
                //    goto Label_0301;
                //}
                //return string.Empty;
                //Label_00B0:
                //if (this.CurrentVisitId < 1L)
                //{
                //    goto Label_0309;
                //}
                //this.CurrentPayer = null;
                //this.CurrentReferringProvider = null;
                //this.CurrentRenderingProvider = null;
                //this.CurrentPayToAddressProvider = null;
                //this.CurrentSupervisingProvider = null;
                //this.CurrentServiceLocation = null;
                //this.FillDataForCurrentClaim();
                //if (!this.IsEqual(this.PreviousProvider, this.CurrentProvider))
                //{
                //    builder.Append(this.GetBillingProviderSegments());
                //    if (this.CurrentPayToAddressProvider != null)
                //    {
                //        builder.Append(this.GetProviderPayToAddressSegments());
                //    }
                //    builder.Append(this.GetSubscriberSegments());
                //    builder.Append(this.GetPayerSegments());
                //}
                //else if (!this.IsEqual(this.PreviousSubscriber, this.CurrentSubscriber))
                //{
                //    builder.Append(this.GetSubscriberSegments());
                //    builder.Append(this.GetPayerSegments());
                //}
                //if (((this.CurrentSubscriber.SBR02.ToString().Trim() != "18") && (this.CurrentSubscriber.SBR02.ToString().Trim() != "")) && !this.IsEqual(this.PreviousPatient, this.CurrentPatient))
                //{
                //    builder.Append(this.GetPatientSegments());
                //}
                //builder.Append(this.GetClaimSegments());
                //if (this.CurrentReferringProvider != null)
                //{
                //    builder.Append(this.GetReferringProviderSegments());
                //}
                //if (this.CurrentRenderingProvider != null)
                //{
                //    builder.Append(this.GetRenderingProviderSegments());
                //}
                //if (this.CurrentServiceLocation != null)
                //{
                //    builder.Append(this.GetServiceLocationSegments());
                //}
                //if (this.CurrentSupervisingProvider != null)
                //{
                //    builder.Append(this.GetSupervisingProviderSegments());
                //}
                //builder.Append(this.GetOthersSubscriberSegments());
                //builder.Append(this.GetServiceLineSegments());
                //this.CopyRow(ref this.PreviousProvider, ref this.CurrentProvider);
                //this.CopyRow(ref this.PreviousSubscriber, ref this.CurrentSubscriber);
                //this.CopyRow(ref this.PreviousPatient, ref this.CurrentPatient);
                //if (this.CurrentVisitId != -1L)
                //{
                //    this.DeleteRow(this.CurrentVisitId);
                //}
                //this.FindNextClaimId();
                //Label_0301:
                //goto Label_00B0;
                //Label_0309:
                //str = builder.ToString();
            }
            catch (Exception exception)
            {
                EDILogger.LogErrorMessage("EDI837Parser::GetLoops", exception);
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetOtherPayer(long PlanProirity, long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((MDVUtility.ToLong(row.PlanProirity) == PlanProirity) && (MDVUtility.ToLong(row.VisitId) == VisitId) && (row.NM101.ToString() == "PR"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetOthersPayerSegments(EDIParser.DSHCFA._837NameRow CurrentOtherPayer)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Name name = new Name(CurrentOtherPayer);
                builder.Append(name.NM1);
                
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetOthersSubscriberSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                EDIParser.DSHCFA._837NameRow[] rowArray = (EDIParser.DSHCFA._837NameRow[])this.DSHCFA._837Name.Select(this.DSHCFA._837Name.PlanProirityColumn.ColumnName + " <> '" + this.CurrentVisitPriority.ToString() + "' AND " + this.DSHCFA._837Name.VisitIdColumn.ColumnName + " = '" + this.CurrentVisitId.ToString() + "' AND " + this.DSHCFA._837Name.NM101Column.ColumnName + " = 'IL' ", this.DSHCFA._837Name.PlanProirityColumn.ColumnName);
                if (rowArray != null)
                {
                    foreach (EDIParser.DSHCFA._837NameRow row in rowArray)
                    {
                        Name name = new Name(row);
                        builder.Append(name.SBR);
                        builder.Append(name.AMT);
                       
                        EDIParser.DSHCFA._837NameRow otherPayer = this.GetOtherPayer(MDVUtility.ToLong(row.PlanProirity), MDVUtility.ToLong(row.VisitId));
                        if (otherPayer != null)
                        {
                            builder.Append(this.GetOthersPayerSegments(otherPayer));
                        }
                    }
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow[] GetOtherSubscriber(long VisitId, long VisitPriority)
        {
            EDIParser.DSHCFA._837NameRow[] rowArray2;
            try
            {
                rowArray2 = (EDIParser.DSHCFA._837NameRow[])this.DSHCFA._837Name.Select(this.DSHCFA._837Name.PlanProirityColumn.ColumnName + " <> '" + VisitPriority.ToString() + "' AND " + this.DSHCFA._837Name.VisitIdColumn.ColumnName + " = '" + VisitId.ToString() + "' AND " + this.DSHCFA._837Name.NM101Column.ColumnName + " = 'IL' ", this.DSHCFA._837Name.PlanProirityColumn.ColumnName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return rowArray2;
        }

        public EDIParser.DSHCFA._837NameRow GetPatient(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetPatientSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.HL_L3());
                Name name = new Name(this.CurrentPatient);
                builder.Append(name.PAT);
                builder.Append(name.NM1);
              
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetPayer(long VisitId, long VisitPriority)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                   
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetPayerSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Name name = new Name(this.CurrentPayer);
                builder.Append(name.NM1);
                if (this.CurrentPayer.N301 != "")
                {
                    builder.Append(name.N3);
                    builder.Append(name.N4);
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetPayToAddressProvider(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        public EDIParser.DSHCFA._837NameRow GetProvider(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((Convert.ToInt64(row.VisitId) == VisitId) && (row.NM101.ToString() == "85"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetProviderPayToAddressSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Name name = new Name(this.CurrentPayToAddressProvider);
                builder.Append(name.NM1);
                builder.Append(name.N3);
               
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetReferringProvider(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((Convert.ToInt64(row.VisitId) == VisitId) && (row.NM101.ToString() == "DN"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetReferringProviderSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (this.CurrentReferringProvider != null)
                {
                    Name name = new Name(this.CurrentReferringProvider);
                    builder.Append(name.NM1);
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetRenderingProvider(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((Convert.ToInt64(row.VisitId) == VisitId) && (row.NM101.ToString() == "82"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetRenderingProviderSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (this.CurrentRenderingProvider != null)
                {
                    Name name = new Name(this.CurrentRenderingProvider);
                    builder.Append(name.NM1);
                   
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetServiceLineSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Queue queue = new Queue();
                EDIParser.DSHCFA._837ServiceLineRow[] rowArray = (EDIParser.DSHCFA._837ServiceLineRow[])this.DSHCFA._837ServiceLine.Select(this.DSHCFA._837ServiceLine.VisitIdColumn.ColumnName + " = '" + this.CurrentVisitId.ToString() + "'", this.DSHCFA._837ServiceLine.CHARGE_ORDERColumn.ColumnName);

                if (rowArray.Length > 0)
                {
                    int indexVal = 0;
                    for (indexVal = 0; indexVal <= (rowArray.Length - 1); indexVal++)
                    {
                        ServiceLine line = new ServiceLine(rowArray[indexVal], indexVal);
                        builder.Append(line.LX);
                     


                        if (!string.IsNullOrEmpty(rowArray[indexVal].LIN03))
                        {
                            builder.Append(line.LIN);
                            builder.Append(line.CTP);
                        }

                        EDIParser.DSHCFA._837SVDServiceLineRow[] rowArray2 = (EDIParser.DSHCFA._837SVDServiceLineRow[])this.DSHCFA._837SVDServiceLine.Select(this.DSHCFA._837SVDServiceLine.VisitIdColumn.ColumnName + " = '" + this.CurrentVisitId.ToString() + "' AND " + this.DSHCFA._837SVDServiceLine.SVD03_2Column.ColumnName + " = '" + rowArray[indexVal].SV101_2.ToString() + "' AND " + this.DSHCFA._837SVDServiceLine.ChargeIdColumn.ColumnName + " = '" + rowArray[indexVal].ChargeId.ToString() + "'");
                        foreach (EDIParser.DSHCFA._837SVDServiceLineRow row in rowArray2)
                        {
                            if (row != null)
                            {
                                SVDServiceLine line2 = new SVDServiceLine(row, rowArray[indexVal]);
                                builder.Append(line2.SVD);
                                builder.Append(line2.CAS_A);
                                builder.Append(line2.CAS_B);
                                builder.Append(line2.CAS_C);
                                builder.Append(line2.CAS_D);
                                if (row.DTP03_A.ToString() != "")
                                {
                                    builder.Append(line2.DTP_A);
                                }
                            }
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    throw new Exception("Claim's Charges Data is not filled properly.");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetServiceLocation(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((Convert.ToInt64(row.VisitId) == VisitId) && (row.NM101.ToString() == "77"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetServiceLocationSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (this.CurrentServiceLocation != null)
                {
                    Name name = new Name(this.CurrentServiceLocation);
                    builder.Append(name.NM1);
                    builder.Append(name.N3);
                    builder.Append(name.N4);
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetSubscriber(long VisitId, long VisitPriority)
        {
            EDIParser.DSHCFA._837NameRow row3;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                EDIParser.DSHCFA._837NameRow[] rowArray = (EDIParser.DSHCFA._837NameRow[])this.DSHCFA._837Name.Select(this.DSHCFA._837Name.VisitIdColumn.ColumnName + " = '" + VisitId.ToString() + "' AND " + this.DSHCFA._837Name.NM101Column.ColumnName + " = 'IL' ", this.DSHCFA._837Name.PlanProirityColumn.ColumnName);
                for (int i = 0; i <= (rowArray.Length - 1); i++)
                {
                    row = rowArray[i];
                    if (i == 0)
                    {
                        row.SBR01 = "P";
                    }
                    else if (i == 1)
                    {
                        row.SBR01 = "S";
                    }
                    else if (i == 2)
                    {
                        row.SBR01 = "T";
                    }
                    
                    else
                    {
                        row.SBR01 = "U";
                    }
                }
                EDIParser.DSHCFA._837NameRow row2 = null;
                for (int j = this.DSHCFA._837Name.Rows.Count - 1; j >= 0; j--)
                {
                    row2 = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[j];
                    
                    row2 = null;
                }
                row3 = row2;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row3;
        }

        private string GetSubscriberSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(this.HL_L2(this.CurrentSubscriber));
                // fixation of issue PMS-798
                Name name = null;
                if (this.CurrentSubscriber.SBR02 != "18")
                    name = new Name(this.CurrentSubscriber, true);
                else
                    name = new Name(this.CurrentSubscriber, false);

                builder.Append(name.SBR);
                builder.Append(name.NM1);
                if (this.CurrentSubscriber.N301 != "")
                {
                    builder.Append(name.N3);
                }
                if (this.CurrentSubscriber.N401 != "")
                {
                    builder.Append(name.N4);
                }
                builder.Append(name.DMG);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        public EDIParser.DSHCFA._837NameRow GetSupervisingProvider(long VisitId)
        {
            EDIParser.DSHCFA._837NameRow row2;
            try
            {
                EDIParser.DSHCFA._837NameRow row = null;
                for (int i = this.DSHCFA._837Name.Rows.Count - 1; i >= 0; i--)
                {
                    row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                    if ((Convert.ToInt64(row.VisitId) == VisitId) && (row.NM101.ToString() == "DQ"))
                    {
                        row = (EDIParser.DSHCFA._837NameRow)this.DSHCFA._837Name.Rows[i];
                        break;
                    }
                    row = null;
                }
                row2 = row;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return row2;
        }

        private string GetSupervisingProviderSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Name name = new Name(this.CurrentSupervisingProvider);
                builder.Append(name.NM1);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string GetTrailerSegments()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                Trailer trailer = new Trailer((EDIParser.DSHCFA._837HeaderRow)this.DSHCFA._837Header.Rows[0]);
               
                builder.Append(trailer.IEA);
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string HL_L1()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("HL*");
                EDICommon.HLBillingProvider = EDICommon.HLCount;
                builder.Append(EDICommon.HLCount);
                EDICommon.HLCount++;
                builder.Append("**20*1");
                builder.Append("~");
                EDICommon.SegCount++;
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string HL_L2(EDIParser.DSHCFA._837NameRow row)
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("HL*");
                EDICommon.HLSubscriber = EDICommon.HLCount;
                builder.Append(EDICommon.HLCount);
                EDICommon.HLCount++;
                builder.Append("*");
                builder.Append(EDICommon.HLBillingProvider);
                builder.Append("*22*");
                bool flag = false;
                if (row.SBR02.ToString().Trim() != "18")
                {
                    builder.Append("1");
                }
                else
                {
                    EDIParser.DSHCFA._837NameRow[] rowArray = (EDIParser.DSHCFA._837NameRow[])this.DSHCFA._837Name.Select(this.DSHCFA._837Name.NM101Column.ColumnName + " = 'IL' AND " + this.DSHCFA._837Name.PAYER_TYPEColumn.ColumnName + "  = '' ");
                    for (int i = 0; i <= (rowArray.Length - 1); i++)
                    {
                        EDIParser.DSHCFA._837NameRow row2 = rowArray[i];
                        if (((((row2.NM103.ToString().Trim() == row.NM103.ToString().Trim()) && (row2.NM104.ToString().Trim() == row.NM104.ToString().Trim())) && (row2.NM109.ToString().Trim() == row.NM109.ToString().Trim())) && (row2.VisitId != row.VisitId)) && (row2.SBR02.ToString().Trim() != "18"))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        builder.Append("1");
                    }
                    else
                    {
                        builder.Append("0");
                    }
                }
                builder.Append("~");
                EDICommon.SegCount++;
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        private string HL_L3()
        {
            string str;
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("HL*");
                builder.Append(EDICommon.HLCount);
                EDICommon.HLCount++;
                builder.Append("*");
                builder.Append(EDICommon.HLSubscriber);
                builder.Append("*23*");
                builder.Append("0");
                builder.Append("~");
                EDICommon.SegCount++;
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str;
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private bool IsEqual(EDIParser.DSHCFA._837NameRow row1, EDIParser.DSHCFA._837NameRow row2)
        {
            bool flag2;
            try
            {
                bool flag = false;
                if ((row1 == null) || (row2 == null))
                {
                    return false;
                }
                if ((row1.RowState == DataRowState.Deleted) || (row2.RowState == DataRowState.Deleted))
                {
                    return false;
                }
                if ((row1["NM103"].ToString().Trim() == row2["NM103"].ToString().Trim()) && (row1["NM104"].ToString().Trim() == row2["NM104"].ToString().Trim()))
                {
                    flag = true;
                }

                if (flag)
                {
                    if (((row1["NM101"].ToString().Trim() == "85") && (row2["NM101"].ToString().Trim() == "85")) && (row1["NM109"].ToString().Trim() == row2["NM109"].ToString().Trim()))
                    {
                        flag = true;
                    }
                    else if (((row1["NM101"].ToString().Trim() == "IL") && (row2["NM101"].ToString().Trim() == "IL")) && (row1["NM109"].ToString().Trim() == row2["NM109"].ToString().Trim()) && row2["IsClaimSplitted"].ToString().Trim() == "1")
                    {
                        flag = false;
                    }
                    else if (((row1["NM101"].ToString().Trim() == "IL") && (row2["NM101"].ToString().Trim() == "IL")) && (row1["NM109"].ToString().Trim() == row2["NM109"].ToString().Trim()))
                    {
                        flag = true;
                    }
                    else if (((row1["NM101"].ToString().Trim() == "PR") && (row2["NM101"].ToString().Trim() == "PR")) && (row1["NM109"].ToString().Trim() == row2["NM109"].ToString().Trim()))
                    {
                        flag = true;
                    }
                    else if (((row1["NM101"].ToString().Trim() == "QC") && (row1["NM101"].ToString().Trim() == "QC")) && (row1["PAT01"].ToString().Trim() == row2["PAT01"].ToString().Trim()))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag2;
        }

        public long visitId { get; set; }
    }
}

