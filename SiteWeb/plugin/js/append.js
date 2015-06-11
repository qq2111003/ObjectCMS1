var inputCheck= new reg();
inputCheck.copyPass=$("#copyPass");
inputCheck.pass=$("#txtPassword");
inputCheck.errorTipDiv=$(".errorTipDiv");
inputCheck.regStructure({
	id:'txtUserName',
	defaultMsg:'4-12位字母和数字',
	isRex:true,
	rexTxt:'^[_0-9a-zA-Z]{4,12}$',
	rexMsg:'账号为4-12位字母和数字',
	isAjax:true,

	ajaxurl:'/HttpHandler/UserCheckHandler.ashx'
})

inputCheck.regStructure({
	id:'txtPassword',
	defaultMsg:'6-12位字母和数字',
	isRex:true,
	rexTxt:'^[_0-9a-zA-Z]{6,12}$',
//	chkcompare : true,
//	compareid: "txtUserName",
	rexMsg:'密码为6-12个字母和数字'
})

inputCheck.regStructure({
	id:'txtEmail',
	defaultMsg:'请输入您的邮箱',
	isRex:true,
	rexTxt:'^[\\w\\.]+([-]\\w+)*@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]',
	rexMsg:'请输入正确的邮箱'
})


inputCheck.regStructure({
	id:'txtRealName',
	defaultMsg:'输入您的真实姓名',
	isRex:true,
	rexTxt:'^[\\u4e00-\\u9fa5]{2,5}$',
	rexMsg:'姓名只能输入2-5个汉字'
})


inputCheck.regStructure({
	id:'txtIdCard',
	defaultMsg:'输入您的身份证号',
	isRex:true,
	rexTxt:'^[0-9]{18}$|^[0-9]{17}[xX]$|^[0-9]{15}$',
	rexMsg: '身份证号输入不正确',
	isAjax:true,
	ajaxurl:'/HttpHandler/IDCardCheckHandler.ashx'
})

inputCheck.regStructure({
	id:'txtValidateCode',
	defaultMsg:'输入验证码',
	isRex:true,
	rexTxt:'^[0-9a-zA-Z]{4}$',
	rexMsg:'验证码错误',
	isAjax:true,
	ajaxurl:'/HttpHandler/AuthCodeCkeckHandler.ashx'
})

inputCheck.regStructure({
	id:'chkProtocol',
	tipMsg:'请先查看服务及隐私条款'
})

$("#btnSubmit").click(function(){
    $(".inforDiv").hide();
    $(".regTipDiv").hide();
	if(inputCheck.checkAll()==true)
	{
	    dsRegAjax();
		return false;
		
		
	}
	else{
	  return false;
	  
	}
	
})




function dsRegAjax()
{
    var inputs=[$('#txtUserName'),$('#txtPassword'),$('#txtEmail'),$('#txtRealName'),$('#txtIdCard'),$('#txtValidateCode')]
    
   $.ajax({
       type: "GET",
       url: "/HttpHandler/RegisterHandler.ashx",
       data: 'userName=' + $("#txtUserName").val() + '&Password=' + $("#txtPassword").val() + '&realName=' + encodeURIComponent($("#txtRealName").val()) + '&email=' + $("#txtEmail").val() + '&cardNo=' + $("#txtIdCard").val() + '&authCode=' + $("#txtValidateCode").val() + "&r=" + Math.random()+'&market='+inputCheck.market,
       beforeSend: function() {
           $(".loadIcon").show();
       },
       success: function(msg) {
           $(".loadIcon").hide();
           $("#vcode").attr("src", "/HttpHandler/AuthCodeHandler.ashx?r=" + Math.random());

               if (parseInt(msg) == 0) {
                   $(".showName").html($("#txtUserName").val())
                   $(".regbox").hide()
                   $(".regcuccbox").show();
                   inputCheck.reSet();
               }
               else if (parseInt(msg) == 1) {
                   $(".errorTipDiv").html(msg.substring(2));
                   $(".errorTipDiv").show();
               }
               else {
                    $(".errorTipDiv").html(msg);
                    $(".errorTipDiv").show();
               }

       },
       complete: function() {

       }
   });
}

 
// validatecode
$("#vcode").click(function() {
    $("#vcode").attr("src", "/HttpHandler/AuthCodeHandler.ashx?r=" + Math.random());
});
$(".regvcodebtn span").click(function() {
    $("#vcode").attr("src", "/HttpHandler/AuthCodeHandler.ashx?r=" + Math.random());
});

$(".reg_check").click(function(){			   
	if($("#chkProtocol").attr("checked"))
	{
		$("#chkProtocol").removeAttr("checked");
		$(this).addClass("reg_check_no")
	}
	else {

		$("#chkProtocol").attr("checked","checked");
		$(".reg_check").removeClass("reg_check_no");
    }
})

$(".regBtnClose").click(function(){
     $(".showName").html("");
    $(".regcuccbox").hide();
    $(".regbox").show();
})