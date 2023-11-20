var _ = {
    currentPageIndex:0,
    MasterData: {},
    options: {
        "url": "/Dictionary/QueryPageData",
        "data": { "PageIndex": 0, "PageSize": 10, "Entity": { "Type": "方向" } },
        "type": "post",
        "datatype": "json",
        "jsonp":"jsonpCallback"
    },
    render: function (data) {
        $("#ListTb tr:gt(0)").remove();
        $("#dataList").tmpl(data).appendTo('#ListTb');
    },
    getQueryString: function (name) {
        var reg = new RegExp('(?:(?:&|\\?)' + name + '=([^&]*))|(?:/' + name + '/([^/]*))', 'i');
        var r = window.location.href.match(reg);
        if (r != null)
            return decodeURI(r[1] || r[2]);
        return null;
    },
    queryList: function (url,data) {
        _.options.url = url;
        _.options.data = data;
        _.ajaxData(_.options, function (result) {
            _.render(result);
            var setTotalCount = result.Total;
            $('#box').paging({
                initPageNo: _.currentPageIndex, // 初始页码
                totalPages: result.TotalPages, //总页数
                totalCount: '合计' + setTotalCount + '条数据', // 条目总数
                slideSpeed: 300, // 缓动速度。单位毫秒
                jump: true, //是否支持跳转
                callback: function (page) {
                    //回调函数
                    _.currentPageIndex = page;
                    _.options.data.PageIndex = page;
                    _.ajaxData(_.options, function (result) {
                        _.render(result);
                    });
                }
            })
        });
    },
    ajaxData: function (options, callback) {
        $.ajax({
            url: options.url,
            data: options.data,
            type: options.type,
            dataType: options.dataType,
            beforeSend: function () {
                $("#loading").show();
            },
            success: function (survey) {
                callback(survey);
            },
            complete: function () {
                $("#loading").hide();
                $('.datetime').datepicker({
                    autoclose: true,
                    format: 'yyyy-mm-dd'
                });
            }
        })
    }
};