using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Wtiau.Health.Web.Models.Domian;
using Wtiau.Health.Web.Models.ViewModels;

namespace Wtiau.Health.Web.Models.Repository
{
    public class Rep_SelectItems
    {
        private readonly HealthEntities db = new HealthEntities();

        public IEnumerable<SelectListItem> Get_AllUnivercity()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_University)
            {
                list.Add(new SelectListItem() { Value = item.University_ID.ToString(), Text = item.University_Display });
            }

            return list.AsEnumerable();
        }

        public IEnumerable<SelectListItem> Get_AllGroup()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_Group)
            {
                list.Add(new SelectListItem() { Value = item.Group_ID.ToString(), Text = item.Group_Display });
            }

            return list.AsEnumerable();
        }


        public IEnumerable<SelectListItem> Get_AllTurn()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_Turn.Where(a => a.Turn_IsActive != false))
            {
                int size = 0;
                int Use = 0;


                foreach (var item2 in item.Tbl_TurnTimeSheet)
                {
                    size += item2.TTS_MaxSize;
                    Use += item2.Tbl_TakeTurn.Count;
                }

                if (size > Use)
                {
                    list.Add(new SelectListItem() { Value = item.Turn_ID.ToString(), Text = item.Turn_Name });

                }

            }

            return list.AsEnumerable();
        }

        public IEnumerable<SelectListItem> Get_AllRole()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_Role)
            {
                list.Add(new SelectListItem() { Value = item.Role_ID.ToString(), Text = item.Role_Display });
            }

            return list.AsEnumerable();
        } 

        public IEnumerable<SelectListItem> Get_AllCourse()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_Course)
            {
                list.Add(new SelectListItem() { Value = item.Course_Guid.ToString(), Text = item.Course_Name });
            }

            return list.AsEnumerable();
        }

        public IEnumerable<SelectListItem> Get_AllStep(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_FormStep.Where(a => a.FS_FormID == id && a.FS_IsDelete == false))
            {
                list.Add(new SelectListItem() { Value = item.FS_ID.ToString(), Text = item.FS_Display });
            }

            return list.AsEnumerable();
        }

        public IEnumerable<SelectListItem> Get_AllForm()
        {
            List<SelectListItem> list = new List<SelectListItem>();


            foreach (var item in db.Tbl_Form.Where(a => a.Form_IsDelete == false))
            {
                list.Add(new SelectListItem() { Value = item.Form_ID.ToString(), Text = item.Form_Name });
            }

            return list.AsEnumerable();
        }
    }
}