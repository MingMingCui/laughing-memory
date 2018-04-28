print('执行 --- hotfix.lua')

fix = {
}

fix.Add = function(a, b)
	return a + b
end

print("Test Add " .. fix.Add(1, 2))

xlua.hotfix(CS.EmoCtrl, 'InitEvent', function(self, open)
	if(open) then
		self.mView.up_btn.onClick:AddListener(function()
			self.mView:ChangeEmo(false)
		end)
		self.mView.down_btn.onClick:AddListener(function()
			self.mView:ChangeEmo(true)
		end)
	else
		self.mView.up_btn.onClick:RemoveAllListeners()
		self.mView.down_btn.onClick:RemoveAllListeners()
	end
end)
