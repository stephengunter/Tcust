class Helper {
   static BusEmitError(error, title) {
      console.log(error)
      let msgtitle = title
      if (error.data && error.data.msg) {
          msgtitle = error.data.msg;
      }
      if (!msgtitle) {
          msgtitle = "系統無回應，請稍後再試"
      }

      Bus.$emit('errors', {
          title: msgtitle,
          status: error.status
      })
  }
  static BusEmitOK(title) {
      let msgtitle = '資料已存檔'
      if (title) msgtitle = title
      let msg = {
          title: msgtitle,
          status: 200
      }

      Bus.$emit('okmsg', msg);
  }
}

export default Helper;