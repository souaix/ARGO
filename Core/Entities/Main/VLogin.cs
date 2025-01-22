using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Main;


public sealed class VLogin
{

    /// <summary>
    /// 使用者ID
    /// </summary>
    [Display(Name = "工號(ID)")]
    [Required(ErrorMessage = "*必填欄位")]
    public string? UserNo { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [Display(Name = "密碼(PWD)")]
    [Required(ErrorMessage = "*必填欄位")]
    public string? Password { get; set; }

    public string? Environment {  get; set; }

}
/// <summary>
/// 解碼字串
/// </summary>
public class Decode_Str
{

    public string? Decode_String { get; set; }
}
/// <summary>
/// 加密字串
/// </summary>
public class Encode_Str
{

    public string? Encode_String { get; set; }
}