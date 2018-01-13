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
	static tryParseInt(val) {
		if (!val) return 0
		return parseInt(val)
   }
   static isTrue(val) {
		if (typeof val == 'number') {
			 return val > 0
		} else if (typeof val == 'string') {
			 if (val.toLowerCase() == 'true') return true
			 if (val == '1') return true
			 return false
		} else if (typeof val == 'boolean') {
			 return val
		}

		return false
	}
	static buildQuery(url, searchParams) {
      url += '?'
      for (let field in searchParams) {

          let value = searchParams[field]
          url += field + '=' + value + '&'

      }
      return url.substr(0, url.length - 1);

   }
}

export default Helper;