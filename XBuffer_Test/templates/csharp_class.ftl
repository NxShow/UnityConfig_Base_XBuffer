namespace xbuffer
{
    // #CLASS_COMMENT#
    public partial #CLASS_TYPE# #CLASS_NAME#
    {
    #VARIABLES#
    #IF_SINGLE#
        public #VAR_TYPE# #VAR_NAME#;				// #VAR_COMMENT#
    #END_SINGLE#
    #IF_ARRAY#
        public #VAR_TYPE#[] #VAR_NAME#;				// #VAR_COMMENT#
    #END_ARRAY#
    #VARIABLES#
    }
}