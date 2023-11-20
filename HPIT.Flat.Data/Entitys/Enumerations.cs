using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.Flat.Data.Entitys
{
    public class Enumerations
    {

        public enum Gender
        {
            [EnumDescription("男")]
            man,
            [EnumDescription("女")]
            woman,
        }

        public enum RequestType
        {
            [EnumDescription("付款")]
            pay,
            [EnumDescription("退款")]
            refund,
        }

        public enum PayRequestStatus
        {
            [EnumDescription("草稿")]
            draft,
            [EnumDescription("审核中")]
            audit,
            [EnumDescription("通过")]
            pass,
            [EnumDescription("完成")]
            complete,
            [EnumDescription("未通过")]
            reject,
            [EnumDescription("作废")]
            cancel,

        }

        public enum RequestStatus
        {
            [EnumDescription("未缴费")]
            none,
            [EnumDescription("缴费中")]
            doing,
            [EnumDescription("完成缴费")]
            done,
        }

        /// <summary>
        /// 数据使用状态
        /// </summary>
        public enum DataStatus
        {
            [EnumDescription("启用")]
            use,
            [EnumDescription("删除")]
            delete
        }

        /// <summary>
        /// 宿舍类型
        /// </summary>
        public enum DormType
        {
            [EnumDescription("男生宿舍")]
            manUse,
            [EnumDescription("女生宿舍")]
            womanUse
        }

        public enum LiveStatus
        {
            [EnumDescription("未入住")]
            none,
            [EnumDescription("已住宿")]
            live,
            [EnumDescription("已退宿")]
            leave
        }

        public enum PayRequestType
        {
            [EnumDescription("缴费")]
            charge,
            [EnumDescription("退宿")]
            leave,
            [EnumDescription("报修")]
            fix
        }

        /// <summary>
        /// 付款类型
        /// </summary>
        public enum PayType
        {
            [EnumDescription("微信")]
            weChat,
            [EnumDescription("支付宝")]
            aliPay,
            [EnumDescription("其他")]
            other
        }

        /// <summary>
        /// 宿舍大小
        /// </summary>
        public enum DormSize
        {
            [EnumDescription("4人间")]
            four,
            [EnumDescription("6人间")]
            six,
            [EnumDescription("8人间")]
            eight
        }

        /// <summary>
        /// 宿舍使用状态 未使用， 部分使用 ，全部使用
        /// </summary>
        public enum DormStatus
        {
            [EnumDescription("未使用")]
            none,
            [EnumDescription("部分使用")]
            part,
            [EnumDescription("全部使用")]
            use
        }

        /// <summary>
        /// 数据使用状态
        /// </summary>
        public enum RoleStatus
        {
            [EnumDescription("启用")]  
            use,
            [EnumDescription("删除")]
            stop
        }
    }
}
