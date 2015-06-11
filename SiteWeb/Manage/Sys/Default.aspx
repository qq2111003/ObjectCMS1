<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="FileManager._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>万能文件管理</title>
    <link href="../plugin/jquery-easyui/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../plugin/jquery-easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../plugin/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <link href="../plugin/CodeMirror/lib/codemirror.css" rel="stylesheet" type="text/css" />
    <link href="../plugin/CodeMirror/theme/rubyblue.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../plugin/CodeMirror/lib/util/simple-hint.css" />
    <style>
        a:hover
        {
            color: #F59000;
        }
        a:active
        {
            outline: none;
        }
        a
        {
            color: #6C6B6C;
            outline: none;
            text-decoration: none;
            border: 0;
        }
        a img
        {
            border: 0;
        }
        .CodeMirror
        {
            border-top: 1px solid black;
            border-bottom: 1px solid black;
        }
        .activeline
        {
            background: #000 !important;
        }
        .CodeMirror
        {
            border: 1px solid #eee;
        }
        .CodeMirror-scroll
        {
            height: auto;
            overflow-y: hidden;
            overflow-x: auto;
        }
    </style>

    <script src="../js/jquery-1.8.0.min.js" type="text/javascript"></script>

    <script src="../plugin/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>

    <script src="../plugin/jquery-easyui/jquery.easyui.min.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/lib/codemirror.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/lib/util/closetag.js" type="text/javascript"></script>

    <script type="text/javascript" src="../plugin/CodeMirror/lib/util/simple-hint.js"></script>

    <script src="../plugin/CodeMirror/lib/util/xml-hint.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/mode/xml/xml.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/mode/javascript/javascript.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/mode/css/css.js" type="text/javascript"></script>

    <script src="../plugin/CodeMirror/mode/htmlmixed/htmlmixed.js" type="text/javascript"></script>

    <script type="text/javascript">

        Array.prototype.hasItem = function(e) {
            for (i = 0; i < this.length; i++) {
                if (this[i] == e)
                    return true;
            }
            return false;
        }

        var Message = {
            show: function(str, title) {
                $.messager.show({
                    title: title || '提示',
                    msg: str,
                    timeout: 3000,
                    showType: 'slide'
                });
            },
            alert: function(str, type, title, func) {
                $.messager.alert(title || '提示', str, type, func);
            },
            confirm: function(str, title, fn) {
                $.messager.confirm(title || '提示', str, function(r) {
                    if (r) {
                        fn();
                    }
                });
            },
            prompt: function(str, title, fn) {
                $.messager.prompt(title || '提示', str, function(t) {
                    if (t) {
                        fn(t);
                    }
                });
            }
        }
        var UIDialog = {
            id: '',
            Temp: '',
            CurrentFrameId: function() {
                return $('.tabs-panels .panel iframe:visible').attr('src');
            },
            Init: function(id, title, width, height, content, isiframe, modal) {
                UIDialog.id = id;
                $('#UIWindowBox').html('<div id="' + id + '" class="easyui-window" minimizable="false" closed="true" modal="true" title="' + title + '" style="width:' + width + 'px;height:' + height + 'px;">' +
                '<div class="easyui-layout" fit="true">' +
                '   <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">' +
                    (isiframe ? '<iframe width="100%" height="100%" id="UIDialog_' + id + '_Iframe" name="UIDialog_' + id + '_Iframe" frameborder="0" src="' + content + '"></iframe>' : content) +
                '   </div>' +
                '   <div id="UIWinBtnBox_' + id + '" region="south" border="false" style="text-align:right;padding:5px 0;">' +
                '   </div>' +
                '</div>' +
                '</div>');
            },
            CreateBtn: function(type, text, fn) {
                $('#UIWinBtnBox_' + UIDialog.id).append('<a id="UIBtn' + type + '" class="easyui-linkbutton" iconCls="icon-' + type + '" href="javascript:void(0)">' + text + '</a>');
                $('#UIBtn' + type).click(function() {
                    fn();
                });
            },
            Open: function() {
                $.parser.parse('#UIWindowBox');
                $('#' + UIDialog.id).window('open');
            },
            Close: function(fn) {
                if (fn) {
                    $('#' + UIDialog.id).window({
                        onClose: function() {
                            fn();
                        }
                    });
                }
                $('#' + UIDialog.id).window('close');
                $('.window:hidden').remove();
                $('.window-shadow:hidden').remove();
                $('.window-mask:hidden').remove();
            }
        }

        function round(v, e) {
            var t = 1;
            for (; e > 0; t *= 10, e--);
            for (; e < 0; t /= 10, e++);
            return Math.round(v * t) / t;
        }
        function FormatFileLength(size) {
            if (1024 > size) {
                return round(size, 2) + "B";
            }
            else if (1024 * 1024 > size) {
                return round(size / 1024, 2) + "KB";
            }
            else if (1024 * 1024 * 1024 > size) {
                return round(size / 1024 / 1024, 2) + "MB";
            }
            else if (1024 * 1024 * 1024 * 1024 > size) {
                return round(size / 1024 / 1024 / 1024, 2) + "GB";
            }
            else {
                return round(size / 1024 / 1024 / 1024 / 1024, 2) + "TB";
            }
        }
        var AjaxController = {
            Post: function(url, data, fn) {
                $.ajax({
                    type: "POST",
                    dataType: "text",
                    url: url,
                    data: data,
                    success: function(msg) {
                        fn(msg);
                    }
                });
            },
            Get: function(url, fn) {
                $.ajax({
                    type: "GET",
                    dataType: "text",
                    url: url,
                    success: function(msg) {
                        fn(msg);
                    }
                });
            }
        }
        function JsonObj2str(o) {
            if (o == undefined) {
                return "";
            }
            var r = [];
            if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
            if (typeof o == "object") {
                if (!o.sort) {
                    for (var i in o)
                        r.push("\"" + i + "\":" + JsonObj2str(o[i]));
                    if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                        r.push("toString:" + o.toString.toString());
                    }
                    r = "{" + r.join() + "}"
                } else {
                    for (var i = 0; i < o.length; i++)
                        r.push(JsonObj2str(o[i]))
                    r = "[" + r.join() + "]";
                }
                return r;
            }
            return o.toString().replace(/\"\:/g, '":""');
        }
    </script>

    <script type="text/javascript">
        var currPathIndex = 0;
        var pathMap = ['/'];
        var queryType = 'ext';
        var queryKeys = '';
        var canEditExt = ['.html', '.htm', '.xml', '.config', '.txt', '.aspx', '.cs', '.css', '.js', '.json'];
        var cantBeDownloadExt = ['文件夹'];
        var canWatchExt = ['.html', 'htm', '.css', '.jpg', '.png', '.gif', '.aspx', '.ashx'];
        var canExtract = ['.zip'];
        var tmpPath = '';
        var tmpPathState = 0;  //0代表无可粘贴内容 1复制 2剪切

        function dosearcher(a, b) {
            queryType = b;
            queryKeys = a;
            LoadData();
        }
        function TitleMap() {
            var tmp = '', fullPath = '';
            var currPathArry = pathMap[currPathIndex].split('/');

            for (var i = 0; i < currPathArry.length - 1; i++) {
                fullPath += '/' + currPathArry[i];
                tmp += '/<a href="javascript:;" onclick="GotoPath(\'' + (fullPath + '/').replace('//', '/') + '\');">' + (currPathArry[i].length == 0 ? '根目录' : currPathArry[i]) + '</a>';
            }
            return tmp;
        }
        function GotoPath(path) {
            queryType = 'ext';
            queryKeys = '';
            if (path == -1) {
                if (currPathIndex > 0) {
                    currPathIndex = currPathIndex - 1;
                    LoadData();
                }
                else {
                    Message.show('已到达起点!', '抱歉');
                }
            } else if (path == 1) {
                if (currPathIndex < pathMap.length - 1) {
                    currPathIndex = currPathIndex + 1;
                    LoadData();
                }
                else {
                    Message.show('已到达终点!', '抱歉');
                }
            } else {
                pathMap.length = currPathIndex + 1;
                pathMap.push(path);
                currPathIndex = currPathIndex + 1;
                LoadData();
            }
        }
        function OpenFile(path) {
            $('#FolderAndFileList').datagrid('loading');
            AjaxController.Post('Default.aspx?method=GetFile', {
                filepath: path
            }, function(data) {
                if (data == 'illegal') {
                    Message.alert("文件路径非法");
                } else if (data == 'notexists') {
                    Message.alert("文件不存在");
                } else {
                    UIDialog.Init('editFile_window', '编辑', 600, 500, [
                            '<div class="easyui-tabs" data-options="fit:true,plain:true">',
				            '   <div title="编辑文件" style="padding:10px;">',
				            '       <div class="easyui-layout" fit="true">',
	                        '           <div data-options="region:\'north\',border:true" style="height:30px;padding:2px">',
	                        '               <input type="text" id="editFileName" readonly="readonly" style="border:1px solid #CCC;width:99%;"  />',
	                        '           </div>',
	                        '           <div data-options="region:\'center\',border:true" style="padding:2px">',
	                        '               <textarea id="editFileText" placeholder="文件内容" style="border:1px solid #CCC;width:98%;height:98%;" height="auto;"></textarea>',
	                        '           </div>',
				            '       </div>',
				            '   </div>',
			                '</div>'
                        ].join('\n'), false);

                    $('#editFileName').val(path.toString().substring(path.toString().lastIndexOf('/') + 1, path.toString().length));


                    var editor = CodeMirror.fromTextArea(document.getElementById("editFileText"), {
                        mode: "text/html",
                        lineNumbers: true,
                        lineWrapping: true,
                        theme: 'rubyblue',
                        extraKeys: {
                            "'>'": function(cm) { cm.closeTag(cm, '>'); },
                            "'/'": function(cm) { cm.closeTag(cm, '/'); }
                        },
                        onCursorActivity: function() {
                            editor.setLineClass(hlLine, null, null);
                            hlLine = editor.setLineClass(editor.getCursor().line, null, "activeline");
                        }
                    });
                    var hlLine = editor.setLineClass(0, "activeline");
                    editor.setValue(data);
                    $('.CodeMirror-scrollbar').hide();

                    UIDialog.CreateBtn('ok', '保存', function() {
                        var FileName = $('#editFileName').val();
                        var FileText = editor.getValue();
                        AjaxController.Post('Default.aspx?method=EditFile', {
                            filename: FileName,
                            filetext: FileText,
                            filepath: pathMap[currPathIndex]
                        }, function(data) {
                            if (data == 'illegal') {
                                Message.alert("文件名非法");
                            } else if (data == 'success') {
                                Message.show("文件已保存");
                                $('#FolderAndFileList').datagrid('reload');
                                UIDialog.Close();
                            } else {
                                Message.alert(data);
                            }
                        });

                    });
                    UIDialog.CreateBtn('cancel', '取消', function() {
                        UIDialog.Close();
                    });
                    UIDialog.Open();
                    $('#FolderAndFileList').datagrid('loaded');
                }
            });

        }
        function LoadData() {
            $('#FolderAndFileList').datagrid('loadData', []);
            $('#FolderAndFileList').datagrid({
                title: [TitleMap(),
                '<div style="float:right;margin-right:30px;margin-top:-3px;">',
                '   <input class="easyui-searchbox" data-options="prompt:\'搜索文件\',menu:\'#mm\',searcher:dosearcher" style="width:220px" value="' + queryKeys + '"/>',
                '   <div id="mm" style="width:120px">',
                '       <div data-options="name:\'ext\'">扩展名</div>',
                '       <div data-options="name:\'file\'">文件名</div>',
                '   </div>',
                '</div>'].join('\n'),
                url: 'Default.aspx?method=GetFolderAndFile&path=' + pathMap[currPathIndex] + '&querykey=' + queryKeys + '&querytype=' + queryType
            });
        }
        $(function() {
            $('#FolderAndFileList').datagrid({
                title: [TitleMap(),
                '<div style="float:right;margin-right:30px;margin-top:-3px;">',
                '   <input class="easyui-searchbox" data-options="prompt:\'搜索文件\',menu:\'#mm\',searcher:dosearcher" style="width:220px"/>',
                '   <div id="mm" style="width:120px">',
                '       <div data-options="name:\'ext\'">扩展名</div>',
                '       <div data-options="name:\'file\'">文件名</div>',
                '   </div>',
                '</div>'].join('\n'),
                fit: true,
                fitColumns: true,
                striped: true,
                singleSelect: false,
                url: 'Default.aspx?method=GetFolderAndFile&path=/',
                idField: 'Name',
                pagination: false, //分页控件
                rownumbers: false, //行号
                columns: [[
                    { field: 'ck', checkbox: true },
	                { field: 'Name', title: '名称', width: 100, align: 'left', editor: 'text',
	                    formatter: function(value, row) {
	                        if (row) {
	                            var fn = 'javascript:;';
	                            if (row.FileType == "文件夹") {
	                                fn = 'GotoPath(\'' + row.Path + '\');';
	                            }
	                            else if (canEditExt.hasItem(row.FileType)) {
	                                fn = 'OpenFile(\'' + row.Path + '\');';
	                            } else {

	                            }
	                            return '<a href="javascript:;" onclick="' + fn + '"><img src="images/fileType/' + row.FileType.replace('.', '') + '.png" onerror="this.src=\'images/fileType/unknown.png\'" width="16px" height="16px"/>' + value + '</a>';
	                        }
	                    }
	                },
	                { field: 'EditTime', title: '修改时间', width: 50, align: 'left' },
	                { field: 'AddTime', title: '添加时间', width: 50, align: 'left' },
					{ field: 'FileType', title: '文件类型', width: 30, align: 'left' },
					{ field: 'FileSize', title: '大小', width: 30, align: 'right', formatter: function(value, row) {
					    if (row) {
					        if (row.FileType == '文件夹') {
					            return "";
					        } else {
					            return FormatFileLength(row.FileSize);
					        }
					    }
					}
					}
                ]],
                toolbar:
            [
                {
                    text: '',
                    iconCls: 'icon-undo',
                    handler: function() {
                        GotoPath(-1);
                    }
                },
                 {
                     text: '',
                     iconCls: 'icon-redo',
                     handler: function() {
                         GotoPath(1);
                     }
                 },
                '-',
                '-',
                '-',
                 {
                     text: '',
                     iconCls: 'icon-add',
                     handler: function() {
                         UIDialog.Init('addFile_window', '添加', 600, 500, [
                            '<div class="easyui-tabs" id="addWindowTabs" data-options="fit:true,plain:true">',
				            '   <div title="添加文件" style="padding:10px;">',
				            '       <div class="easyui-layout" fit="true">',
	                        '           <div data-options="region:\'north\',border:true" style="height:30px;padding:2px">',
	                        '               <input type="text" id="addFileName" placeholder="请输入文件名" style="border:1px solid #CCC;width:98%;"/>',
	                        '           </div>',
	                        '           <div data-options="region:\'center\',border:true" style="padding:2px">',
	                        '               <textarea id="addFileText" placeholder="文件内容" style="border:1px solid #CCC;width:98%;height:98%;" height="auto"></textarea>',
	                        '           </div>',
				            '       </div>',
				            '   </div>',
				            '   <div title="上传文件" style="padding:10px;"> ',
				            '       <div class="easyui-layout" fit="true">',
	                        '           <div data-options="region:\'center\',border:true" style="padding:2px">',
	                        '               <div id="fileQueue"></div>',
	                        '           </div>',
	                        '           <div data-options="region:\'south\',border:true" style="height:50px;padding:2px">',
                            '               <center><input type="file" name="uploadify" id="uploadify" /></center>',
	                        '           </div>',
				            '       </div>',
				            '   </div>',
				            '   <div title="添加文件夹" style="padding:10px;">',
				            '       <div class="easyui-layout" fit="true">',
	                        '           <div data-options="region:\'north\',border:true" style="height:30px;padding:2px">',
	                        '               <input type="text" id="addFolderName" placeholder="请输入文件夹名" style="border:1px solid #CCC;width:95%;"/>',
	                        '           </div>',
	                        '           <div data-options="region:\'center\',border:true" style="padding:2px">',
	                        '               ',
	                        '           </div>',
				            '       </div>',
				            '   </div>',
			                '</div>'
                        ].join('\n'), false);
                         var editor;
                         UIDialog.CreateBtn('ok', '完成', function() {
                             var currTab = $('#addWindowTabs').tabs('getSelected').panel('options').title;
                             if (currTab == '添加文件') {
                                 var FileName = $('#addFileName').val();
                                 var FileText = editor.getValue();
                                 AjaxController.Post('Default.aspx?method=AddFile', {
                                     filename: FileName,
                                     filetext: FileText,
                                     filepath: pathMap[currPathIndex]
                                 }, function(data) {
                                     if (data == 'illegal') {
                                         Message.alert("文件名非法");
                                     } else if (data == 'exists') {
                                         Message.alert("文件已存在");
                                     } else if (data == 'success') {
                                         Message.show("文件已保存");
                                         $('#FolderAndFileList').datagrid('reload');
                                         UIDialog.Close();
                                     } else {
                                         Message.alert(data);
                                     }
                                 });
                             } else if (currTab == '上传文件') {
                                 $("#uploadify").uploadify('upload', '*');
                             } else {
                                 var folderName = $('#addFolderName').val();
                                 AjaxController.Post('Default.aspx?method=AddFolder', {
                                     foldername: folderName,
                                     path: pathMap[currPathIndex]
                                 }, function(data) {
                                     if (data == 'illegal') {
                                         Message.alert("文件夹名非法");
                                     } else if (data == 'exists') {
                                         Message.alert("文件夹已存在");
                                     } else if (data == 'success') {
                                         Message.show("文件夹已创建");
                                         $('#FolderAndFileList').datagrid('reload');
                                         UIDialog.Close();
                                     } else {
                                         Message.alert(data);
                                     }
                                 });
                             }

                         });
                         UIDialog.CreateBtn('cancel', '取消', function() {
                             UIDialog.Close();
                         });
                         UIDialog.Open();

                         $("#uploadify").uploadify({
                             swf: '../plugin/uploadify/uploadify.swf',
                             uploader: 'Default.aspx?method=UploadFile',
                             formData: { forder: pathMap[currPathIndex] },
                             queueID: 'fileQueue',
                             buttonText: '选择文件',
                             auto: false,
                             multi: true,
                             onQueueComplete: function(a) {
                                 $('#FolderAndFileList').datagrid('reload');
                                 UIDialog.Close();
                             }
                         });
                         //text/html
                         editor = CodeMirror.fromTextArea(document.getElementById("addFileText"), {
                             mode: "text/html",
                             lineNumbers: true,
                             lineWrapping: true,
                             theme: 'rubyblue',
                             extraKeys: {
                                 "'>'": function(cm) { cm.closeTag(cm, '>'); },
                                 "'/'": function(cm) { cm.closeTag(cm, '/'); }
                             },
                             onCursorActivity: function() {
                                 editor.setLineClass(hlLine, null, null);
                                 hlLine = editor.setLineClass(editor.getCursor().line, null, "activeline");
                             }
                         });
                         var hlLine = editor.setLineClass(0, "activeline");
                         $('.CodeMirror-scrollbar').hide();
                         editor.setValue('\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n');
                     }
                 },
                {
                    text: '',
                    iconCls: 'icon-cancel',
                    handler: function() {
                        var row = $('#FolderAndFileList').datagrid('getSelections');
                        if (row.length != 0) {
                            var delarry = '';
                            for (var i = 0; i < row.length; i++) {
                                if (row[i].Path != pathMap[currPathIndex])
                                    delarry += '|' + row[i].Path;
                            }
                            delarry = delarry.substr(1);

                            //防止误操作 先禁用删除功能
                            AjaxController.Post('Default.aspx?method=Del', {
                                delrow: delarry
                            }, function(data) {
                                if (data == 'success') {
                                    Message.show("已删除");
                                    $('#FolderAndFileList').datagrid('reload');
                                } else {
                                    Message.alert(data);
                                }
                            });
                        } else {
                            Message.alert('请选择要删除的ROW');
                        }
                    }
                },
                {
                    text: '',
                    iconCls: 'icon-reload',
                    handler: function() {
                        $('#FolderAndFileList').datagrid('reload');
                    }
                },
                '-',
                '-',
                '-',
                {
                    text: '复制',
                    handler: function() {
                        var row = $('#FolderAndFileList').datagrid('getSelections');
                        if (row.length != 0) {
                            var copyArry = '';
                            for (var i = 0; i < row.length; i++) {
                                if (row[i].Path != pathMap[currPathIndex])
                                    copyArry += '|' + row[i].Path;
                            }
                            copyArry = copyArry.substr(1);
                            tmpPath = copyArry;
                            tmpPathState = 1;
                        } else {
                            Message.alert('请选择要复制的ROW');
                        }
                    }
                },
                {
                    text: '剪切',
                    handler: function() {
                        var row = $('#FolderAndFileList').datagrid('getSelections');
                        if (row.length != 0) {
                            var copyArry = '';
                            for (var i = 0; i < row.length; i++) {
                                if (row[i].Path != pathMap[currPathIndex])
                                    copyArry += '|' + row[i].Path;
                            }
                            copyArry = copyArry.substr(1);
                            tmpPath = copyArry;
                            tmpPathState = 2;
                        } else {
                            Message.alert('请选择要剪切的ROW');
                        }
                    }
                },
                {
                    text: '粘贴',
                    handler: function() {
                        //ajax -> C# ->  file.moveto
                        if (tmpPathState != 0) {
                            AjaxController.Post('Default.aspx?method=CopyTo', {
                                paths: tmpPath,
                                currpath: pathMap[currPathIndex],
                                copytype: tmpPathState
                            }, function(data) {
                                if (data == 'illegal') {
                                    Message.alert("路径非法", '错误', 'error');
                                } else if (data == 'notexists') {
                                    Message.alert("已存在同名文件或目录", '错误', 'error');
                                } else if (data == "success") {
                                    Message.show("完成!");
                                    $('#FolderAndFileList').datagrid('reload');

                                    tmpPath = '';
                                    tmpPathState = 0;
                                }
                            });
                        } else {
                            Message.alert("剪切栏是空的", '错误', 'error');
                        }
                    }
                },
                '-',
                '-',
                '-',
                {
                    text: '压缩',
                    handler: function() {
                        var row = $('#FolderAndFileList').datagrid('getSelections');
                        if (row.length != 0) {
                            Message.prompt('请输入压缩到的文件名', 'zip文件名', function(zipName) {
                                var zipArry = '';
                                for (var i = 0; i < row.length; i++) {
                                    if (row[i].Path != pathMap[currPathIndex])
                                        zipArry += '|' + row[i].Path;
                                }
                                zipArry = zipArry.substr(1);
                                AjaxController.Post('Default.aspx?method=ToZip', {
                                    sourcepaths: zipArry,
                                    currpath: pathMap[currPathIndex],
                                    zipfilename: zipName
                                }, function(data) {
                                    if (data == "success") {
                                        Message.show("压缩完成!");
                                        $('#FolderAndFileList').datagrid('reload');
                                    }
                                });
                            });

                        } else {
                            Message.alert('请选择要压缩的ROW', '错误', 'error');
                        }
                    }
                },
                {
                    text: '解压',
                    handler: function() {
                        var row = $('#FolderAndFileList').datagrid('getSelections');
                        if (row.length == 0) {
                            Message.alert('请选择要压缩的ROW', '错误', 'error');
                        } else {
                            var zipArry = '';
                            if (row[0].Path != pathMap[currPathIndex]) {
                                zipArry = row[0].Path;
                            } else {
                                zipArry = row[1].Path;
                            }
                            AjaxController.Post('Default.aspx?method=EXZip', {
                                zippaths: zipArry,
                                currpath: pathMap[currPathIndex]
                            }, function(data) {
                                if (data == "success") {
                                    Message.show("解压完成!");
                                    $('#FolderAndFileList').datagrid('reload');
                                }
                            });

                        }
                    }
}],
                    onLoadSuccess: function() {
                    },
                    onRowContextMenu: function(e, i, row) {
                        e.preventDefault();
                        $(this).datagrid('unselectAll');
                        $(this).datagrid('selectRow', i);
                        $('#FolderAndFileMenu').menu('show', {
                            left: e.pageX + 2,
                            top: e.pageY + 2
                        });
                        //显隐
                        if (tmpPathState == 0) {
                            var item = $("#FolderAndFileMenu").menu("findItem", "粘贴");
                            $("#FolderAndFileMenu").menu("disableItem", item.target);
                        } else {
                            var item = $("#FolderAndFileMenu").menu("findItem", "粘贴");
                            $("#FolderAndFileMenu").menu("enableItem", item.target);
                        }

                        if (cantBeDownloadExt.hasItem(row.FileType)) {
                            var item = $("#FolderAndFileMenu").menu("findItem", "下载");
                            $("#FolderAndFileMenu").menu("disableItem", item.target);
                        } else {
                            var downloadItem = $("#FolderAndFileMenu").menu("findItem", "下载");
                            $("#FolderAndFileMenu").menu("enableItem", downloadItem.target);
                        }

                        if (!canWatchExt.hasItem(row.FileType)) {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "浏览");
                            $("#FolderAndFileMenu").menu("disableItem", WatchItem.target);
                        } else {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "浏览");
                            $("#FolderAndFileMenu").menu("enableItem", WatchItem.target);
                        }

                        if (canEditExt.hasItem(row.FileType) || row.FileType == '文件夹') {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "打开");
                            $("#FolderAndFileMenu").menu("enableItem", WatchItem.target);
                        } else {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "打开");
                            $("#FolderAndFileMenu").menu("disableItem", WatchItem.target);
                        }
                        if (canExtract.hasItem(row.FileType)) {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "压缩");
                            if (!WatchItem) {
                                WatchItem = $("#FolderAndFileMenu").menu("findItem", "解压缩");
                            }
                            $("#FolderAndFileMenu").menu("setText", { text: '解压缩', target: WatchItem.target });
                        } else {
                            var WatchItem = $("#FolderAndFileMenu").menu("findItem", "压缩");
                            if (!WatchItem) {
                                WatchItem = $("#FolderAndFileMenu").menu("findItem", "解压缩");
                            }
                            $("#FolderAndFileMenu").menu("setText", { text: '压缩', target: WatchItem.target });
                        }

                        //绑定事件
                        $('#FolderAndFileMenu').menu({
                            onClick: function(item) {
                                if (item.text == '浏览') {
                                    window.open(row.Path);
                                }
                                else if (item.text == '打开') {
                                    if (row.FileType == "文件夹") {
                                        GotoPath(row.Path);
                                    }
                                    else {
                                        OpenFile(row.Path);
                                    }
                                } else if (item.text == '压缩') {
                                    AjaxController.Post('Default.aspx?method=ToZip', {
                                        sourcepaths: row.Path,
                                        currpath: pathMap[currPathIndex],
                                        zipfilename: (row.FileType == '文件夹' ? row.Name : row.Name.substr(0, row.Name.indexOf('.'))) + '.zip'
                                    }, function(data) {
                                        if (data == "success") {
                                            Message.show("压缩完成!");
                                            $('#FolderAndFileList').datagrid('reload');
                                        }
                                    });
                                } else if (item.text == '解压缩') {
                                    AjaxController.Post('Default.aspx?method=EXZip', {
                                        zippaths: row.Path,
                                        currpath: pathMap[currPathIndex]
                                    }, function(data) {
                                        if (data == "success") {
                                            Message.show("解压完成!");
                                            $('#FolderAndFileList').datagrid('reload');
                                        }
                                    });
                                }
                                else if (item.text == '复制') {
                                    tmpPath = row.Path;
                                    tmpPathState = 1;
                                } else if (item.text == '剪切') {
                                    tmpPath = row.Path;
                                    tmpPathState = 2;
                                }
                                else if (item.text == '粘贴') {
                                    //ajax -> C# ->  file.moveto
                                    AjaxController.Post('Default.aspx?method=CopyTo', {
                                        paths: tmpPath,
                                        currpath: pathMap[currPathIndex],
                                        copytype: tmpPathState
                                    }, function(data) {
                                        if (data == 'illegal') {
                                            Message.alert("路径非法", '错误', 'error');
                                        } else if (data == 'notexists') {
                                            Message.alert("已存在同名文件或目录", '错误', 'error');
                                        } else if (data == "success") {
                                            Message.show("完成!");
                                            $('#FolderAndFileList').datagrid('reload');
                                            tmpPath = '';
                                            tmpPathState = 0;
                                        }
                                    });
                                }
                                else if (item.text == '下载') {
                                    $('#downloadFrame').attr('src', 'Default.aspx?method=Download&Path=' + encodeURIComponent(row.Path)
                                    + '&Name=' + encodeURIComponent(row.Name));
                                } else if (item.text == '重命名') {
                                    $('#FolderAndFileList').datagrid('beginEdit', i);
                                    $('.datagrid-editable-input').focus();
                                    $('.datagrid-editable-input').select();
                                    $('.datagrid-editable-input').blur(function() {
                                        $('#FolderAndFileList').datagrid('endEdit', i);
                                    });
                                } else if (item.text == '删除') {
                                    AjaxController.Post('Default.aspx?method=Del', {
                                        delrow: row.Path
                                    }, function(data) {
                                        if (data == 'success') {
                                            Message.show("已删除");
                                            $('#FolderAndFileList').datagrid('reload');
                                        } else {
                                            Message.alert(data);
                                        }
                                    });
                                }
                            }
                        });
                    },
                    onBeforeLoad: function() {
                        $.parser.parse('.panel-title');
                        $('#FolderAndFileList').datagrid('clearSelections');
                    },
                    onLoadSuccess: function() {

                    },
                    onLoadError: function() {
                        Message.show('加载出错!');
                    },
                    onAfterEdit: function(rowIndex, rowData, changes) {
                        AjaxController.Post('Default.aspx?method=ChangeName', rowData, function(data) {
                            if (data == 'illegal') {
                                Message.alert("路径非法", '错误', 'error', function() {
                                    $('#FolderAndFileList').datagrid('beginEdit', rowIndex);
                                    $('.datagrid-editable-input').focus();
                                    $('.datagrid-editable-input').select();
                                    $('.datagrid-editable-input').blur(function() {
                                        $('#FolderAndFileList').datagrid('endEdit', rowIndex);
                                    });
                                });
                            } else if (data == 'notexists') {
                                Message.alert("已存在同名文件或目录", '错误', 'error', function() {
                                    $('#FolderAndFileList').datagrid('beginEdit', rowIndex);
                                    $('.datagrid-editable-input').focus();
                                    $('.datagrid-editable-input').select();
                                    $('.datagrid-editable-input').blur(function() {
                                        $('#FolderAndFileList').datagrid('endEdit', rowIndex);
                                    });
                                });
                            } else if (data == "success") {
                                Message.show("重命名完成!");
                                $('#FolderAndFileList').datagrid('reload');
                            }
                        });
                    }
                });
            });
    </script>

</head>
<body>
    <iframe id="downloadFrame" width="0" height="0" border="0" style="display:none;"></iframe>
    <div id="UIWindowBox">
    </div>
    <table id="FolderAndFileList">
    </table>
    <div id="FolderAndFileMenu" class="easyui-menu" style="width: 120px;">
        <div>
            浏览
        </div>
        <div>
            打开
        </div>
        <div>
            压缩
        </div>
        <div>
            下载
        </div>
        <div>
            剪切
        </div>
        <div>
            复制
        </div>
        <div>
            粘贴
        </div>
        <div>
            重命名
        </div>
        <div>
            删除</div>
    </div>
</body>
</html>
