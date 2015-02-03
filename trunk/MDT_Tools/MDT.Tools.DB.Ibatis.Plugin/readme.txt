1.根据数据表菜单中选中的表信息，模板以及配置文件./control/MDT_ORA.xml生成相应的ibatis。
2.暂时将po的Package设置为ats.common.model.po, dao的Package设置为ats.common.model.dao；可考虑配置化。
3.配置文件./control/MDT_ORA.xml来判断该表是否存在自动生成的列；后续可以考虑简化该文件内容,配置文件路径配置化。
4.生成文件对应的模板需要放到templates目录下。