// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: EEANEHJJCNG.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace EggLink.DanhengServer.Proto {

  /// <summary>Holder for reflection information generated from EEANEHJJCNG.proto</summary>
  public static partial class EEANEHJJCNGReflection {

    #region Descriptor
    /// <summary>File descriptor for EEANEHJJCNG.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EEANEHJJCNGReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFFRUFORUhKSkNORy5wcm90bxoNTWlzc2lvbi5wcm90byJdCgtFRUFORUhK",
            "SkNORxIkChJtaXNzaW9uX2V2ZW50X2xpc3QYDSADKAsyCC5NaXNzaW9uEhMK",
            "C0VBTEpGQ0VOS0pNGAkgAygNEhMKC0xFRkJPSk1QRUVKGA8gAygNQh6qAhtF",
            "Z2dMaW5rLkRhbmhlbmdTZXJ2ZXIuUHJvdG9iBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::EggLink.DanhengServer.Proto.MissionReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::EggLink.DanhengServer.Proto.EEANEHJJCNG), global::EggLink.DanhengServer.Proto.EEANEHJJCNG.Parser, new[]{ "MissionEventList", "EALJFCENKJM", "LEFBOJMPEEJ" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class EEANEHJJCNG : pb::IMessage<EEANEHJJCNG>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<EEANEHJJCNG> _parser = new pb::MessageParser<EEANEHJJCNG>(() => new EEANEHJJCNG());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<EEANEHJJCNG> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::EggLink.DanhengServer.Proto.EEANEHJJCNGReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public EEANEHJJCNG() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public EEANEHJJCNG(EEANEHJJCNG other) : this() {
      missionEventList_ = other.missionEventList_.Clone();
      eALJFCENKJM_ = other.eALJFCENKJM_.Clone();
      lEFBOJMPEEJ_ = other.lEFBOJMPEEJ_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public EEANEHJJCNG Clone() {
      return new EEANEHJJCNG(this);
    }

    /// <summary>Field number for the "mission_event_list" field.</summary>
    public const int MissionEventListFieldNumber = 13;
    private static readonly pb::FieldCodec<global::EggLink.DanhengServer.Proto.Mission> _repeated_missionEventList_codec
        = pb::FieldCodec.ForMessage(106, global::EggLink.DanhengServer.Proto.Mission.Parser);
    private readonly pbc::RepeatedField<global::EggLink.DanhengServer.Proto.Mission> missionEventList_ = new pbc::RepeatedField<global::EggLink.DanhengServer.Proto.Mission>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::EggLink.DanhengServer.Proto.Mission> MissionEventList {
      get { return missionEventList_; }
    }

    /// <summary>Field number for the "EALJFCENKJM" field.</summary>
    public const int EALJFCENKJMFieldNumber = 9;
    private static readonly pb::FieldCodec<uint> _repeated_eALJFCENKJM_codec
        = pb::FieldCodec.ForUInt32(74);
    private readonly pbc::RepeatedField<uint> eALJFCENKJM_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> EALJFCENKJM {
      get { return eALJFCENKJM_; }
    }

    /// <summary>Field number for the "LEFBOJMPEEJ" field.</summary>
    public const int LEFBOJMPEEJFieldNumber = 15;
    private static readonly pb::FieldCodec<uint> _repeated_lEFBOJMPEEJ_codec
        = pb::FieldCodec.ForUInt32(122);
    private readonly pbc::RepeatedField<uint> lEFBOJMPEEJ_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> LEFBOJMPEEJ {
      get { return lEFBOJMPEEJ_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as EEANEHJJCNG);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(EEANEHJJCNG other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!missionEventList_.Equals(other.missionEventList_)) return false;
      if(!eALJFCENKJM_.Equals(other.eALJFCENKJM_)) return false;
      if(!lEFBOJMPEEJ_.Equals(other.lEFBOJMPEEJ_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= missionEventList_.GetHashCode();
      hash ^= eALJFCENKJM_.GetHashCode();
      hash ^= lEFBOJMPEEJ_.GetHashCode();
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
      eALJFCENKJM_.WriteTo(output, _repeated_eALJFCENKJM_codec);
      missionEventList_.WriteTo(output, _repeated_missionEventList_codec);
      lEFBOJMPEEJ_.WriteTo(output, _repeated_lEFBOJMPEEJ_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      eALJFCENKJM_.WriteTo(ref output, _repeated_eALJFCENKJM_codec);
      missionEventList_.WriteTo(ref output, _repeated_missionEventList_codec);
      lEFBOJMPEEJ_.WriteTo(ref output, _repeated_lEFBOJMPEEJ_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      size += missionEventList_.CalculateSize(_repeated_missionEventList_codec);
      size += eALJFCENKJM_.CalculateSize(_repeated_eALJFCENKJM_codec);
      size += lEFBOJMPEEJ_.CalculateSize(_repeated_lEFBOJMPEEJ_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(EEANEHJJCNG other) {
      if (other == null) {
        return;
      }
      missionEventList_.Add(other.missionEventList_);
      eALJFCENKJM_.Add(other.eALJFCENKJM_);
      lEFBOJMPEEJ_.Add(other.lEFBOJMPEEJ_);
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
          case 74:
          case 72: {
            eALJFCENKJM_.AddEntriesFrom(input, _repeated_eALJFCENKJM_codec);
            break;
          }
          case 106: {
            missionEventList_.AddEntriesFrom(input, _repeated_missionEventList_codec);
            break;
          }
          case 122:
          case 120: {
            lEFBOJMPEEJ_.AddEntriesFrom(input, _repeated_lEFBOJMPEEJ_codec);
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
          case 74:
          case 72: {
            eALJFCENKJM_.AddEntriesFrom(ref input, _repeated_eALJFCENKJM_codec);
            break;
          }
          case 106: {
            missionEventList_.AddEntriesFrom(ref input, _repeated_missionEventList_codec);
            break;
          }
          case 122:
          case 120: {
            lEFBOJMPEEJ_.AddEntriesFrom(ref input, _repeated_lEFBOJMPEEJ_codec);
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