// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: FKHOCCFAPLK.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace EggLink.DanhengServer.Proto {

  /// <summary>Holder for reflection information generated from FKHOCCFAPLK.proto</summary>
  public static partial class FKHOCCFAPLKReflection {

    #region Descriptor
    /// <summary>File descriptor for FKHOCCFAPLK.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static FKHOCCFAPLKReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFGS0hPQ0NGQVBMSy5wcm90byJiCgtGS0hPQ0NGQVBMSxITCgtFSUVCTkhK",
            "S0tEQRgJIAEoCBIXCg9pc190YWtlbl9yZXdhcmQYCCABKAgSEwoLRkZKQURN",
            "S0RBQlAYBCABKA0SEAoIcGFuZWxfaWQYCyABKA1CHqoCG0VnZ0xpbmsuRGFu",
            "aGVuZ1NlcnZlci5Qcm90b2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::EggLink.DanhengServer.Proto.FKHOCCFAPLK), global::EggLink.DanhengServer.Proto.FKHOCCFAPLK.Parser, new[]{ "EIEBNHJKKDA", "IsTakenReward", "FFJADMKDABP", "PanelId" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class FKHOCCFAPLK : pb::IMessage<FKHOCCFAPLK>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FKHOCCFAPLK> _parser = new pb::MessageParser<FKHOCCFAPLK>(() => new FKHOCCFAPLK());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FKHOCCFAPLK> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::EggLink.DanhengServer.Proto.FKHOCCFAPLKReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FKHOCCFAPLK() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FKHOCCFAPLK(FKHOCCFAPLK other) : this() {
      eIEBNHJKKDA_ = other.eIEBNHJKKDA_;
      isTakenReward_ = other.isTakenReward_;
      fFJADMKDABP_ = other.fFJADMKDABP_;
      panelId_ = other.panelId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FKHOCCFAPLK Clone() {
      return new FKHOCCFAPLK(this);
    }

    /// <summary>Field number for the "EIEBNHJKKDA" field.</summary>
    public const int EIEBNHJKKDAFieldNumber = 9;
    private bool eIEBNHJKKDA_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool EIEBNHJKKDA {
      get { return eIEBNHJKKDA_; }
      set {
        eIEBNHJKKDA_ = value;
      }
    }

    /// <summary>Field number for the "is_taken_reward" field.</summary>
    public const int IsTakenRewardFieldNumber = 8;
    private bool isTakenReward_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool IsTakenReward {
      get { return isTakenReward_; }
      set {
        isTakenReward_ = value;
      }
    }

    /// <summary>Field number for the "FFJADMKDABP" field.</summary>
    public const int FFJADMKDABPFieldNumber = 4;
    private uint fFJADMKDABP_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint FFJADMKDABP {
      get { return fFJADMKDABP_; }
      set {
        fFJADMKDABP_ = value;
      }
    }

    /// <summary>Field number for the "panel_id" field.</summary>
    public const int PanelIdFieldNumber = 11;
    private uint panelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint PanelId {
      get { return panelId_; }
      set {
        panelId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as FKHOCCFAPLK);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(FKHOCCFAPLK other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (EIEBNHJKKDA != other.EIEBNHJKKDA) return false;
      if (IsTakenReward != other.IsTakenReward) return false;
      if (FFJADMKDABP != other.FFJADMKDABP) return false;
      if (PanelId != other.PanelId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (EIEBNHJKKDA != false) hash ^= EIEBNHJKKDA.GetHashCode();
      if (IsTakenReward != false) hash ^= IsTakenReward.GetHashCode();
      if (FFJADMKDABP != 0) hash ^= FFJADMKDABP.GetHashCode();
      if (PanelId != 0) hash ^= PanelId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (FFJADMKDABP != 0) {
        output.WriteRawTag(32);
        output.WriteUInt32(FFJADMKDABP);
      }
      if (IsTakenReward != false) {
        output.WriteRawTag(64);
        output.WriteBool(IsTakenReward);
      }
      if (EIEBNHJKKDA != false) {
        output.WriteRawTag(72);
        output.WriteBool(EIEBNHJKKDA);
      }
      if (PanelId != 0) {
        output.WriteRawTag(88);
        output.WriteUInt32(PanelId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (FFJADMKDABP != 0) {
        output.WriteRawTag(32);
        output.WriteUInt32(FFJADMKDABP);
      }
      if (IsTakenReward != false) {
        output.WriteRawTag(64);
        output.WriteBool(IsTakenReward);
      }
      if (EIEBNHJKKDA != false) {
        output.WriteRawTag(72);
        output.WriteBool(EIEBNHJKKDA);
      }
      if (PanelId != 0) {
        output.WriteRawTag(88);
        output.WriteUInt32(PanelId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (EIEBNHJKKDA != false) {
        size += 1 + 1;
      }
      if (IsTakenReward != false) {
        size += 1 + 1;
      }
      if (FFJADMKDABP != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(FFJADMKDABP);
      }
      if (PanelId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(PanelId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(FKHOCCFAPLK other) {
      if (other == null) {
        return;
      }
      if (other.EIEBNHJKKDA != false) {
        EIEBNHJKKDA = other.EIEBNHJKKDA;
      }
      if (other.IsTakenReward != false) {
        IsTakenReward = other.IsTakenReward;
      }
      if (other.FFJADMKDABP != 0) {
        FFJADMKDABP = other.FFJADMKDABP;
      }
      if (other.PanelId != 0) {
        PanelId = other.PanelId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 32: {
            FFJADMKDABP = input.ReadUInt32();
            break;
          }
          case 64: {
            IsTakenReward = input.ReadBool();
            break;
          }
          case 72: {
            EIEBNHJKKDA = input.ReadBool();
            break;
          }
          case 88: {
            PanelId = input.ReadUInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 32: {
            FFJADMKDABP = input.ReadUInt32();
            break;
          }
          case 64: {
            IsTakenReward = input.ReadBool();
            break;
          }
          case 72: {
            EIEBNHJKKDA = input.ReadBool();
            break;
          }
          case 88: {
            PanelId = input.ReadUInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code